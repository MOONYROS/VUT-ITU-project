using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace WpfApp1.App.Converters;
public class ColorToBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var tmpvalue = (System.Drawing.Color)(value);
		Color wpfColor = Color.FromArgb(tmpvalue.A, tmpvalue.R, tmpvalue.G, tmpvalue.B);
		return new SolidColorBrush(wpfColor);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DependencyProperty.UnsetValue;
	}
}
