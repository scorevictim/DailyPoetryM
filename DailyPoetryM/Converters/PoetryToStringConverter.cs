using DailyPoetryM.Models;
using System.Globalization;

namespace DailyPoetryM.Converters;

public class PoetryToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not Poetry poetry ? throw new Exception() : $"{poetry.Dynasty} {poetry.Author} {poetry.Snippet}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
