using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
