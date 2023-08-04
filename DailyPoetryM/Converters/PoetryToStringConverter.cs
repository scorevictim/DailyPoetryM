using DailyPoetryM.Models;
using System.Globalization;

namespace DailyPoetryM.Converters;

public class PoetryToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Poetry poetry ? $"{poetry.Dynasty} {poetry.Author} {poetry.Snippet}" : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
