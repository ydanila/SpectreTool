using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SpectreTool.ServiceDTO.Data;

namespace SpectreTool.Console
{
	//	original code: https://gallery.technet.microsoft.com/scriptcenter/Speculation-Control-e36f0050

	internal static class SpeculationControl
	{
		[DllImport("ntdll.dll")]
		public static extern int NtQuerySystemInformation(uint systemInformationClass, IntPtr systemInformation,
			uint systemInformationLength, IntPtr returnLength);

		public static void Fill(CheckResult checkResult)
		{
			var systemInformationPtr = Marshal.AllocHGlobal(4);
			var returnLengthPtr = Marshal.AllocHGlobal(4);

			try
			{
				//	Spectre
				CheckCVE_2017_5715(systemInformationPtr, returnLengthPtr, checkResult);

				//	Meltdown
				CheckCVE_2017_5754(systemInformationPtr, returnLengthPtr, checkResult);

				//PrintGuidance(btiHardwarePresent, btiWindowsSupportPresent, btiWindowsSupportEnabled, kvaShadowRequired,
				//	kvaShadowPresent, kvaShadowEnabled);
			}
			finally
			{
				if (systemInformationPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(systemInformationPtr);
				}

				if (returnLengthPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(returnLengthPtr);
				}
			}
		}

		private static void PrintGuidance(bool btiHardwarePresent, bool btiWindowsSupportPresent,
			bool btiWindowsSupportEnabled, bool kvaShadowRequired, bool kvaShadowPresent, bool kvaShadowEnabled)
		{
			//
			// Provide guidance as appropriate.
			//

			var actions = new List<string>();
			if (btiHardwarePresent == false)
				actions.Add(
					"Install BIOS/firmware update provided by your device OEM that enables hardware support for the branch target injection mitigation.");

			if (btiWindowsSupportPresent == false || kvaShadowPresent == false)
				actions.Add("Install the latest available updates for Windows with support for speculation control mitigations.");

			if (btiHardwarePresent && btiWindowsSupportEnabled == false || kvaShadowRequired && kvaShadowEnabled == false)
			{
				var guidanceUri = "";
				var guidanceType = "";


				//	$os = Get-WmiObject Win32_OperatingSystem
				var os = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().First();

				if ((int)os["ProductType"] == 1)
				{
					// Workstation
					guidanceUri = "https://support.microsoft.com/help/4073119";
					guidanceType = "Client";
				}
				else
				{
					// Server/DC
					guidanceUri = "https://support.microsoft.com/help/4072698";
					guidanceType = "Server";
				}

				actions.Add(
					$"Follow the guidance for enabling Windows {guidanceType} support for speculation control mitigations described in {guidanceUri}");
			}

			if (actions.Count > 0)
			{
				Debug.WriteLine("");
				Debug.WriteLine("Suggested actions"); // -ForegroundColor Cyan
				Debug.WriteLine("");

				foreach (var action in actions) Debug.WriteLine(" *" + action);
			}
		}

		private static void CheckCVE_2017_5754(IntPtr systemInformationPtr, IntPtr returnLengthPtr, CheckResult checkResult)
		{
			//
			// Query kernel VA shadow information.
			//

			checkResult.KvaShadowRequired = true;
			checkResult.KvaShadowPresent = false;
			checkResult.KvaShadowEnabled = false;
			checkResult.KvaShadowPcidEnabled = false;

			var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
			var manufacturer = cpu["Manufacturer"];

			if (manufacturer.Equals("AuthenticAMD"))
			{
				checkResult.KvaShadowRequired = false;
			}
			else if (manufacturer.Equals("GenuineIntel"))
			{
				var regex = new Regex("Family (\\d+) Model (\\d+) Stepping (\\d+)");
				var result = regex.Match((string) cpu["Description"]);

				if (result.Success)
				{
					var family = uint.Parse(result.Groups[1].Value);
					var model = uint.Parse(result.Groups[2].Value);
					var stepping = uint.Parse(result.Groups[3].Value);

					if (family == 0x6 &&
					    (model == 0x1c || model == 0x26 || model == 0x27 || model == 0x36 || model == 0x35))
					{
						checkResult.KvaShadowRequired = false;
					}
				}
			}
			else
			{
				throw new Exception($"Unsupported processor manufacturer: {manufacturer}");
			}

			uint systemInformationClass = 196;
			uint systemInformationLength = 4;

			var retval = NtQuerySystemInformation(systemInformationClass, systemInformationPtr, systemInformationLength, returnLengthPtr);

			if (retval == unchecked((int) 0xc0000003) || retval == unchecked((int) 0xc0000002))
			{
			}
			else if (retval != 0)
			{
				throw new Exception($"Querying kernel VA shadow information failed with error {retval:X8}");
			}
			else
			{
				uint kvaShadowEnabledFlag = 0x01;
				uint kvaShadowUserGlobalFlag = 0x02;
				uint kvaShadowPcidFlag = 0x04;
				uint kvaShadowInvpcidFlag = 0x08;

				var flags = (uint) Marshal.ReadInt32(systemInformationPtr);

				checkResult.KvaShadowPresent = true;
				checkResult.KvaShadowEnabled = (flags & kvaShadowEnabledFlag) != 0;
				checkResult.KvaShadowPcidEnabled = (flags & kvaShadowPcidFlag) != 0 && (flags & kvaShadowInvpcidFlag) != 0;

				checkResult.KvaShadowUserGlobal = (flags & kvaShadowUserGlobalFlag) != 0;
				checkResult.KvaShadowPcid = (flags & kvaShadowPcidFlag) != 0;
				checkResult.KvaShadowInvpcid = (flags & kvaShadowInvpcidFlag) != 0;
			}
		}

		private static void CheckCVE_2017_5715(IntPtr systemInformationPtr, IntPtr returnLengthPtr, CheckResult result)
		{
			//
			// Query branch target injection information.
			//
			result.BtiHardwarePresent = false;
			result.BtiWindowsSupportPresent = false;
			result.BtiWindowsSupportEnabled = false;
			result.BtiDisabledBySystemPolicy = false;
			result.BtiDisabledByNoHardwareSupport = false;

			uint systemInformationClass = 201;
			uint systemInformationLength = 4;

			var retval = NtQuerySystemInformation(systemInformationClass, systemInformationPtr, systemInformationLength,
				returnLengthPtr);

			if (retval == unchecked((int) 0xc0000003) || retval == unchecked((int) 0xc0000002))
			{
				// fallthrough
			}
			else if (retval != 0)
			{
				throw new Exception($"Querying branch target injection information failed with error {retval:X8}");
			}
			else
			{
				uint scfBpbEnabled = 0x01;
				uint scfBpbDisabledSystemPolicy = 0x02;
				uint scfBpbDisabledNoHardwareSupport = 0x04;
				uint scfHwReg1Enumerated = 0x08;
				uint scfHwReg2Enumerated = 0x10;
				uint scfHwMode1Present = 0x20;
				uint scfHwMode2Present = 0x40;
				uint scfSmepPresent = 0x80;

				var flags = (uint) Marshal.ReadInt32(systemInformationPtr);

				result.BtiHardwarePresent = (flags & scfHwReg1Enumerated) != 0 || (flags & scfHwReg2Enumerated) != 0;
				result.BtiWindowsSupportPresent = true;

				result.BtiWindowsSupportEnabled = (flags & scfBpbEnabled) != 0;

				if (!result.BtiWindowsSupportEnabled)
				{
					result.BtiDisabledBySystemPolicy = (flags & scfBpbDisabledSystemPolicy) != 0;
					result.BtiDisabledByNoHardwareSupport = (flags & scfBpbDisabledNoHardwareSupport) != 0;
				}

				result.BpbEnabled = (flags & scfBpbEnabled) != 0;
				result.BpbDisabledSystemPolicy = (flags & scfBpbDisabledSystemPolicy) != 0;
				result.BpbDisabledNoHardwareSupport = (flags & scfBpbDisabledNoHardwareSupport) != 0;
				result.HwReg1Enumerated = (flags & scfHwReg1Enumerated) != 0;
				result.HwReg2Enumerated = (flags & scfHwReg2Enumerated) != 0;
				result.HwMode1Present = (flags & scfHwMode1Present) != 0;
				result.HwMode2Present = (flags & scfHwMode2Present) != 0;
				result.SmepPresent = (flags & scfSmepPresent) != 0;
			}
		}
	}
}