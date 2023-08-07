using DailyPoetryM.Models;
using System.Globalization;

namespace DailyPoetryM.Converters;

public class ItemTappedEventArgsToPoetryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as ItemTappedEventArgs)?.Item as Poetry;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
