using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.Services.Validations
{
    class NotNullValidation : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);
            if (value == null)
                return result;
            if(value.ToString() == "")
            {
                return new ValidationResult(false, this.ErrorMessage);
            }
            return result;
        }
    }
}
