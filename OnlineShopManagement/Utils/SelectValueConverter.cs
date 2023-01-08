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
            if (value != null && parameter != null)
            {
                int rtvalue = (int)value;
                int rtpar = int.Parse(parameter.ToString());
                return (rtvalue == rtpar) ? Brushes.White : Brushes.Black;
            }
            return Brushes.Black;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

