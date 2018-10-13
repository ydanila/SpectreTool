using System;
using System.Globalization;
using SpectreTool.Views.Extensions.Interfaces;

namespace SpectreTool.Views.Extensions.Converters
{
	public class AndBooleanConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var result = true;

			foreach(var val in values)
			{
				bool converted;
				switch (val)
				{
					case null:
						//	Binding.DoNothing
						//	DependencyProperty.UnsetValue 
						//return new object();
						converted = false;
						break;

					case bool _:
						converted = (bool)val;
						break;

					default:
						throw new ArgumentException("Not boolean values not supported");
				}

				result = result && converted;
			}

			return result;
		}
	}
}
