using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1.App.Converters;

public class MediaColorToDrawingColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is System.Drawing.Color drawingColor)
		{
			return System.Windows.Media.Color.FromArgb(
				drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
		}

		return DependencyProperty.UnsetValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is System.Windows.Media.Color wpfColor)
		{
			return System.Drawing.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
		}

		return DependencyProperty.UnsetValue;
	}
}