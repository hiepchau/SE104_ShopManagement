using SE104_project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SE104_OnlineShopManagement.Services
{
    public class MainWindowNavigator<TWindow> : INavigator where TWindow : Window
    {
		private readonly TWindow _windowProvider;

		public MainWindowNavigator(TWindow _windowProvider)
		{
			this._windowProvider = _windowProvider;
		}

		public void Navigate()
        {
			Window oldWindow = App.Current.MainWindow;
			Window newWindow = (Window)_windowProvider;
			newWindow.Show();
			App.Current.MainWindow = newWindow;
            //oldWindow?.Close();
        }
    }
}
