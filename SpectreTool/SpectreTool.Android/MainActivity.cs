using Android.App;
using Android.Content.PM;
using Android.OS;
using HtmlLabel.Forms.Plugin.Droid;
using SpectreTool.Android;
using SpectreTool.Android.ViewModel;
using SpectreTool.Model;
using SpectreTool.Model.Interfaces;
using SpectreTool.ViewModel.Interfaces;

namespace SpectreTool.Android
{
	[Activity(Label = "SpectreTool", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

			HtmlLabelRenderer.Initialize();
			global::Xamarin.Forms.Forms.Init(this, bundle);
			LoadApplication(new App(new AndroidInitializer()));
        }

		public class AndroidInitializer : IPlatformInitializer
		{
			public void RegisterTypes()
			{
				DependencyLocator.Register<ILocalize, Localize>(DependencyLocatorTarget.GlobalInstance);
			}
		}
	}
}