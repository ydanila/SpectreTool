using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SpectreTool.Model;
using SpectreTool.Model.Interfaces;
using SpectreTool.ServiceDTO;
using SpectreTool.ServiceDTO.Command;
using SpectreTool.ServiceDTO.Data;
using SpectreTool.UWP.Model;
using HtmlLabel.Forms.Plugin.UWP;

namespace SpectreTool.UWP
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : Application
    {
		private BackgroundTaskDeferral m_appServiceDeferral;
		private AppServiceConnection m_appServiceConnection;
		private AutoResetEvent m_serviceConnected;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
        {
			InitializeComponent();
			Suspending += OnSuspending;

			m_serviceConnected = new AutoResetEvent(false);
		}

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

				var rendererAssemblies = new[]
				{
					typeof(HtmlLabelRenderer).GetTypeInfo().Assembly
				};
				Xamarin.Forms.Forms.Init(e, rendererAssemblies);
				HtmlLabelRenderer.Initialize();

				DependencyLocator.Register<IInfoProvider, InfoProvider>();

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
		
		internal async Task<CheckResult> CheckAsync()
		{
			await LaunchProcessIfRequired();

			return await CallProcessCommand<CheckCommand, CheckResult>();
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

		protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
		{
			base.OnBackgroundActivated(args);
			IBackgroundTaskInstance taskInstance = args.TaskInstance;
			var appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
			m_appServiceDeferral = taskInstance.GetDeferral();
			taskInstance.Canceled += OnAppServicesCanceled;
			m_appServiceConnection = appService.AppServiceConnection;
			m_appServiceConnection.RequestReceived += OnAppServiceRequestReceived;
			m_appServiceConnection.ServiceClosed += AppServiceConnection_ServiceClosed;
			m_serviceConnected.Set();
		}

		private async void OnAppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
		{
			AppServiceDeferral messageDeferral = args.GetDeferral();
			messageDeferral.Complete();
		}

		private void OnAppServicesCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			m_appServiceDeferral.Complete();
		}

		private void AppServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
		{
			m_appServiceDeferral.Complete();
		}

		private async Task LaunchProcessIfRequired()
		{
			if(m_appServiceConnection != null)
			{
				return;
			}

			await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

			await Task.Factory.StartNew(() => { m_serviceConnected.WaitOne(); });
		}

		private async Task<TRes> CallProcessCommand<TCom, TRes>() where TCom : BaseCommand where TRes : BaseCommandResult, new()
		{
			var response = await m_appServiceConnection.SendMessageAsync(MessagingUtils.PackCommand<ValueSet>(new CheckCommand()));			
			if(response.Status != AppServiceResponseStatus.Success)
			{
				return new TRes()
				{
					IsError = true
				};
			}

			var result = MessagingUtils.UnpackResult<BaseCommandResult>(response.Message);
			if(result is TRes)
			{
				return (TRes)result;
			}

			return new TRes()
			{
				IsError = true
			};
		}
	}
}