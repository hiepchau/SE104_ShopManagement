using SE104_OnlineShopManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    public class LanguageViewModel : ViewModelBase
    {
        public static LanguageViewModel Instance;
        public LanguageViewModel()
        {
            Instance = this;
        }

        public string CurrentLanguage
        {
            get => Language.getCurrentLanguage();
            set => OnPropertyChanged(nameof(CurrentLanguage));
        }
    }
}
