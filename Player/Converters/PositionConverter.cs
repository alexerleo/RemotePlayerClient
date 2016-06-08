using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Player.Converters
{
    public class PositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            TimeSpan val = (TimeSpan)value;
            return val.TotalMilliseconds;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
