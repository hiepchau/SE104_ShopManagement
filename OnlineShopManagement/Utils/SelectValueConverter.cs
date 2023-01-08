using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace OnlineShopManagement.Utils
{
    public class SelectValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter != null)
            {
                int rtvalue = (int)value;
                int rtpar = (int)parameter;
                return (rtvalue == rtpar) ? Colors.White : Colors.Black;
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

