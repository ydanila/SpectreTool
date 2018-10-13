namespace SpectreTool.ServiceDTO.Data
{
	public class CheckResult : BaseCommandResult
	{
		public bool BtiHardwarePresent { get; set; }

		public bool BtiWindowsSupportPresent { get; set; }

		public bool BtiWindowsSupportEnabled { get; set; }

		public bool BtiNotEnabled => BtiWindowsSupportPresent && !BtiWindowsSupportEnabled;

		public bool BtiDisabledBySystemPolicy { get; set; }

		public bool BtiDisabledByNoHardwareSupport { get; set; }

		public bool BpbEnabled { get; set; }

		public bool BpbDisabledSystemPolicy { get; set; }

		public bool BpbDisabledNoHardwareSupport { get; set; }

		public bool HwReg1Enumerated { get; set; }

		public bool HwReg2Enumerated { get; set; }

		public bool HwMode1Present { get; set; }

		public bool HwMode2Present { get; set; }

		public bool SmepPresent { get; set; }

		public bool KvaShadowRequired { get; set; }

		public bool KvaShadowNotRequired => !KvaShadowRequired;

		public bool KvaShadowPresent { get; set; }

		public bool KvaShadowEnabled { get; set; }

		public bool KvaShadowPcidEnabled { get; set; }

		public bool KvaShadowUserGlobal { get; set; }

		public bool KvaShadowPcid { get; set; }

		public bool KvaShadowInvpcid { get; set; }

		public string OsName { get; set; }

		public string OsVersion { get; set; }

		public string CpuName { get; set; }

		public string CpuModel { get; set; }

		public string CpuRevision { get; set; }

		public string BiosName { get; set; }

		public string BiosManufacturer { get; set; }

		public string BiosReleaseDate { get; set; }
	}
}