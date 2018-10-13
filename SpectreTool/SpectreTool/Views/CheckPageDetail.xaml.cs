using SpectreTool.ViewModel.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckPageDetail : ContentPage
	{
		public CheckPageDetail ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MessagingCenter.Send(new CheckDeviceMessage(), MessagesKeys.CheckDevice);
		}
	}
}