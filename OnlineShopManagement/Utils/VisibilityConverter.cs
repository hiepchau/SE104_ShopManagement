using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SE104_OnlineShopManagement.Utils
{
    public class VisibilityConverter : IValueConverter
    {
        #region VisibilityConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool source;
            if(value != null)
            {
                source = (bool)value;
                if (source)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
