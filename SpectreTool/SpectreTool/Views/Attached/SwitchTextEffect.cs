using System.Linq;
using Xamarin.Forms;

namespace SpectreTool.Views.Attached
{
    public static class SwitchTextEffect
    {
	    public static readonly BindableProperty OnTextProperty =
		    BindableProperty.CreateAttached("OnText", typeof(string), typeof(SwitchTextEffect), string.Empty, propertyChanged: OnOffTextPropertyChanged);

	    public static readonly BindableProperty OffTextProperty =
		    BindableProperty.CreateAttached("OffText", typeof(string), typeof(SwitchTextEffect), string.Empty, propertyChanged: OnOffTextPropertyChanged);

	    public static string GetOnText(BindableObject view)
	    {
		    return (string)view.GetValue(OnTextProperty);
	    }

	    public static void SetOnText(BindableObject view, string value)
	    {
		    view.SetValue(OnTextProperty, value);
	    }

	    public static string GetOffText(BindableObject view)
	    {
		    return (string)view.GetValue(OffTextProperty);
	    }

	    public static void SetOffText(BindableObject view, string value)
	    {
		    view.SetValue(OffTextProperty, value);
	    }

		static void OnOffTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	    {
		    var view = bindable as View;
		    if (view == null)
		    {
			    return;
		    }

		    var effect = view.Effects.FirstOrDefault(e => e is SwitchTextEffectRouting);
		    if (effect == null)
		    {
			    effect = new SwitchTextEffectRouting();
				view.Effects.Add(effect);
		    }
		}

	    class SwitchTextEffectRouting : RoutingEffect
	    {
		    public SwitchTextEffectRouting() : base("SpectreTool.SwitchTextEffect")
		    {
		    }
	    }
	}
}
