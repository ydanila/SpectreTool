using System.ComponentModel;
using SpectreTool.UWP.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("SpectreTool")]
[assembly: ExportEffect(typeof(SwitchTextEffect), "SwitchTextEffect")]
namespace SpectreTool.UWP.View
{
	public class SwitchTextEffect : PlatformEffect
	{
		private static readonly BindableProperty OnTextProperty = Views.Attached.SwitchTextEffect.OnTextProperty;
		private static readonly BindableProperty OffTextProperty = Views.Attached.SwitchTextEffect.OffTextProperty;

		protected override void OnAttached()
		{
			UpdateOnText();
			UpdateOffText();
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == OnTextProperty.PropertyName)
			{
				UpdateOnText();
				return;
			}

			if (args.PropertyName == OffTextProperty.PropertyName)
			{
				UpdateOffText();
				return;
			}
		}

		private void UpdateOnText()
		{
			if (!Element.IsSet(OnTextProperty))
			{
				return;
			}

			var text = Views.Attached.SwitchTextEffect.GetOnText(Element);
			((Windows.UI.Xaml.Controls.ToggleSwitch) Control).OnContent = text;
		}

		private void UpdateOffText()
		{
			if (!Element.IsSet(OffTextProperty))
			{
				return;
			}

			var text = Views.Attached.SwitchTextEffect.GetOffText(Element);
			((Windows.UI.Xaml.Controls.ToggleSwitch)Control).OffContent = text;
		}
	}
}