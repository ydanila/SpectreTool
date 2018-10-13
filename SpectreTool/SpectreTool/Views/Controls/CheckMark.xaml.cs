using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views.Controls
{
	/// <summary>
	/// Icons:
	/// UWP:
	/// https://www.flaticon.com/free-icon/tick-inside-circle_61222
	/// https://www.flaticon.com/free-icon/circle-outline_60722
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckMark : ContentView
	{
		public static readonly BindableProperty CheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckMark), false);

		public bool IsChecked
		{
			get { return (bool) GetValue(CheckedProperty); }
			set
			{
				SetValue(CheckedProperty, value);
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsNotChecked));
			}
		}

		public bool IsNotChecked
		{
			get { return !IsChecked; }
		}

		public CheckMark ()
		{
			InitializeComponent ();
		}
	}
}