using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApp1.App.Views;

public class UrlImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var imageUrl = value as string;
		if (!string.IsNullOrEmpty(imageUrl))
		{
			return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
		}

		return DependencyProperty.UnsetValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();	
	}
}