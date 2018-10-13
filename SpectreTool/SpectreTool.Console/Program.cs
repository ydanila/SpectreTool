using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using SpectreTool.ServiceDTO;
using SpectreTool.ServiceDTO.Command;
using SpectreTool.ServiceDTO.Data;

namespace SpectreTool.Console
{
	internal static class Program
	{
		private static AutoResetEvent m_appServiceExit;
		private static AppServiceConnection m_inventoryService;

		private static void Main()
		{
			MainAsync().Wait();
		}

		private static async Task MainAsync()
		{
#if DEBUG
			//Debugger.Launch();
#endif
			m_appServiceExit = new AutoResetEvent(false);
			
			m_inventoryService = new AppServiceConnection
			{
				AppServiceName = "InProcessAppService",
				PackageFamilyName = "ec0cc741-fd3e-485c-81be-68815c480690_z0pasavz61fvy"
			};
			m_inventoryService.RequestReceived += InventoryService_RequestReceived;
			m_inventoryService.ServiceClosed += InventoryService_ServiceClosed;

			var status = await m_inventoryService.OpenAsync();

			if (status == AppServiceConnectionStatus.Success)
			{
				m_appServiceExit.WaitOne();
			}
		}

		private static async void InventoryService_RequestReceived(AppServiceConnection sender,
			AppServiceRequestReceivedEventArgs args)
		{
			BaseCommandResult result = null;

			var command = MessagingUtils.UnpackCommand<BaseCommand>(args.Request.Message);
			if (command is CheckCommand)
			{
				var checkResult = new CheckResult();
				SysInfo.Fill(checkResult);
				SpeculationControl.Fill(checkResult);

				result = checkResult;
			}
			else
			{
				result = new ErrorResult();
			}

			await args.Request.SendResponseAsync(MessagingUtils.PackResult<ValueSet>(result));

			//	close app after each command
			m_appServiceExit.Set();
		}

		private static void InventoryService_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
		{
			m_appServiceExit.Set();
		}

		private static void Main1(string[] args)
		{
			//var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();

			//SpeculationControl.Check(true);

			//var inventoryService = new AppServiceConnection();

			//// Here, we use the app service name defined in the app service provider's Package.appxmanifest file in the <Extension> section.
			//inventoryService.AppServiceName = "InProcessAppService";

			//// Use Windows.ApplicationModel.Package.Current.Id.FamilyName within the app service provider to get this value.
			//inventoryService.PackageFamilyName = "ec0cc741-fd3e-485c-81be-68815c480690_z0pasavz61fvy";

			//System.Console.WriteLine("...");

			//var task = Task.Factory.StartNew(async () => {
			//	try
			//	{
			//		var status = await inventoryService.OpenAsync();

			//		System.Console.WriteLine(status);

			//		if(status == AppServiceConnectionStatus.Success)
			//		{
			//			ValueSet valueSet = new ValueSet();
			//			valueSet.Add("Request", "aaaaa");

			//			var response = await inventoryService.SendMessageAsync(valueSet);
			//			System.Console.WriteLine(response);
			//			if(response.Status == AppServiceResponseStatus.Success)
			//			{
			//				//string responseMessage = response.Message["response"].ToString();
			//				//if(responseMessage == "success")
			//				//{
			//				//	this.Hide();
			//				//}
			//			}
			//		}
			//	}
			//	catch(Exception ex)
			//	{
			//		System.Console.WriteLine(ex.ToString());
			//	}
			//});

			//task.Wait();

			//System.Console.ReadLine();

			//		dynamic CPU = new { };

			//		CPU.ID = (string)cpu["ProcessorId"];
			//		CPU.Socket = (string)cpu["SocketDesignation"];
			//		CPU.Name = (string)cpu["Name"];
			//		CPU.Description = (string)cpu["Caption"];
			//		CPU.AddressWidth = (ushort)cpu["AddressWidth"];
			//		CPU.DataWidth = (ushort)cpu["DataWidth"];
			//		//CPU.Architecture = (CPU.CpuArchitecture)(ushort)cpu["Architecture"];
			//		CPU.Architecture = (ushort)cpu["Architecture"];
			//		CPU.SpeedMHz = (uint)cpu["MaxClockSpeed"];
			//		CPU.BusSpeedMHz = (uint)cpu["ExtClock"];
			//		CPU.L2Cache = (uint)cpu["L2CacheSize"] * (ulong)1024;
			//		CPU.L3Cache = (uint)cpu["L3CacheSize"] * (ulong)1024;
			//		CPU.Cores = (uint)cpu["NumberOfCores"];
			//		CPU.Threads = (uint)cpu["NumberOfLogicalProcessors"];

			//		CPU.Name =
			//		   CPU.Name
			//		   .Replace("(TM)", "™")
			//		   .Replace("(tm)", "™")
			//		   .Replace("(R)", "®")
			//		   .Replace("(r)", "®")
			//		   .Replace("(C)", "©")
			//		   .Replace("(c)", "©")
			//		   .Replace("    ", " ")
			//		   .Replace("  ", " ");
		}
	}
}