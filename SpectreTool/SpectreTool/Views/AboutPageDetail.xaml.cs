using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPageDetail : ContentPage
	{
		public AboutPageDetail ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//NavigationPage.SetHasBackButton(this, false);
			//NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}