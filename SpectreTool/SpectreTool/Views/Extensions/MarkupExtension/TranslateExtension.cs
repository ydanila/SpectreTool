using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using SpectreTool.Model;
using SpectreTool.ViewModel.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views.Extensions.MarkupExtension
{
	[ContentProperty("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		private static readonly string ResourceId = typeof(SpectreTool.ViewModel.Localization.UI).FullName;
		private readonly CultureInfo ci;

		private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

		public TranslateExtension()
		{
			if(Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
			{
				ci = DependencyLocator.Get<ILocalize>().GetCurrentCultureInfo();
			}
		}

		public string Text { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if(Text == null)
			{
				return "";
			}

			var translation = ResMgr.Value.GetString(Text, ci);

			if(translation == null)
			{
#if DEBUG
				throw new ArgumentException(String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name), "Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
			}
			return translation;
		}
	}
}
