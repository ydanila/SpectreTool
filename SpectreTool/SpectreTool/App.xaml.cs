using SpectreTool.Model;
using SpectreTool.Model.Interfaces;
using SpectreTool.ViewModel;
using SpectreTool.ViewModel.Messages;
using SpectreTool.Views;
using Xamarin.Forms;

namespace SpectreTool
{
	public partial class App : Application
	{
		private MasterDetailRoot m_masterDetailRoot;

		public App(IPlatformInitializer initializer = null)
		{
			InitializeComponent();

			if (initializer != null)
			{
				initializer.RegisterTypes();
			}
			ViewModelLocator.RegisterViews();

			m_masterDetailRoot = new MasterDetailRoot();
			MainPage = m_masterDetailRoot;
		}

		protected override void OnStart ()
		{
			MessagingCenter.Send<ShowViewMessage>(new ShowViewMessage { View = ViewType.Main }, MessagesKeys.ShowView);
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
