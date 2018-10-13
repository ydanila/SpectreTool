using System;
using System.Globalization;

namespace SpectreTool.Views.Extensions.Interfaces
{
	/// <summary>
	/// Source: https://github.com/Keboo/Xamarin.Forms.Proxy/blob/master/Xamarin.Forms.Proxy/IMultiValueConverter.cs
	/// </summary>
	public interface IMultiValueConverter
	{
		object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
	}
}
