using System.Globalization;

namespace DailyPoetryM.Converters;

public class BytesToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is byte[] bytes ? ImageSource.FromStream(() => new MemoryStream(bytes)) : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
