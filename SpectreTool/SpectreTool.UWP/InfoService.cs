﻿using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace SpectreTool.UWP
{
	/// <summary>
	/// https://docs.microsoft.com/en-us/windows/uwp/launch-resume/how-to-create-and-consume-an-app-service
	/// </summary>
	public class InfoService : IBackgroundTask
	{
		private BackgroundTaskDeferral backgroundTaskDeferral;
		private AppServiceConnection appServiceconnection;

		public void Run(IBackgroundTaskInstance taskInstance)
		{
			this.backgroundTaskDeferral = taskInstance.GetDeferral(); // Get a deferral so that the service isn't terminated.
			taskInstance.Canceled += OnTaskCanceled; // Associate a cancellation handler with the background task.

			// Retrieve the app service connection and set up a listener for incoming app service requests.
			var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			appServiceconnection = details.AppServiceConnection;
			appServiceconnection.RequestReceived += OnRequestReceived;
		}

		private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			// Get a deferral because we use an awaitable API below to respond to the message
			// and we don't want this call to get cancelled while we are waiting.
			var messageDeferral = args.GetDeferral();

			ValueSet message = args.Request.Message;
		}

		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			if(this.backgroundTaskDeferral != null)
			{
				// Complete the service deferral.
				this.backgroundTaskDeferral.Complete();
			}
		}
	}
}
