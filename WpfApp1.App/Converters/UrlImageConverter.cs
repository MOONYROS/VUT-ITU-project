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
			try
			{
				return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
			}
			catch (Exception e)
			{
				return new BitmapImage(
					new Uri(
						"https://us-tuna-sounds-images.voicemod.net/d814859a-f807-43d0-96b7-1db93a715498-1662477059618.jpg",
					UriKind.Absolute));
			}
		}
		return new BitmapImage(
			new Uri(
				"https://us-tuna-sounds-images.voicemod.net/d814859a-f807-43d0-96b7-1db93a715498-1662477059618.jpg",
				UriKind.Absolute));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();	
	}
}