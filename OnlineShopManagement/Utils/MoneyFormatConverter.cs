using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SE104_OnlineShopManagement.Utils
{
    public class MoneyFormatConverter : IValueConverter

    {
        #region MoneyFormatConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = String.Empty;
            if(value != null && value is long)
            {
                s = String.Format(new CultureInfo("en-US"), "{0:n0}", (long)value);
                return s;
            }
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long result;
            if(value != null)
            {
                result = ConvertToNumber(value.ToString());
                return result;
            }
            return null;
        }
        public long ConvertToNumber(string str)
        {
            long result;
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            if(long.TryParse(tmp,out result))
            {
                return result;
            }
            return result;
        }
        #endregion
    }
}
