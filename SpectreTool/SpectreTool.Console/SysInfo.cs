using System.Linq;
using System.Management;
using SpectreTool.ServiceDTO.Data;

namespace SpectreTool.Console
{
	static class SysInfo
	{
		internal static void Fill(CheckResult result)
		{
			var os = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().First();
			result.OsName = ReadPropertyValue(os, "Caption");
			result.OsVersion = ReadPropertyValue(os, "Version");

			var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
			result.CpuName = ReadPropertyValue(cpu, "Name");
			result.CpuModel = ReadPropertyValue(cpu, "Description");
			result.CpuRevision = ReadPropertyValue(cpu, "Revision");

			var bios = new ManagementObjectSearcher("select * from Win32_BIOS").Get().Cast<ManagementObject>().First();
			result.BiosName = ReadPropertyValue(bios, "Name");
			result.BiosManufacturer = ReadPropertyValue(bios, "Manufacturer");
			var dateStr = ReadPropertyValue(bios, "ReleaseDate");
			result.BiosReleaseDate = ManagementDateTimeConverter.ToDateTime(dateStr).ToShortDateString();
		}

		private static string ReadPropertyValue(ManagementObject obj, string key)
		{
			var val = obj.GetPropertyValue(key);
			if(val == null)
			{
				return string.Empty;
			}

			return val.ToString();
		}
	}
}
