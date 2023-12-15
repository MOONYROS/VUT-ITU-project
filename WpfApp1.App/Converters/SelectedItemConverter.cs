using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using WpfApp1.BL.Models;

namespace WpfApp1.App.Converters;

public class SelectedItemConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is TagDetailModel item && parameter is ObservableCollection<TagDetailModel> selectedTags)
		{
			return selectedTags.Contains(item);
		}

		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool isChecked && isChecked && parameter is TagDetailModel item)
		{
			return item;
		}

		return null;
	}
}