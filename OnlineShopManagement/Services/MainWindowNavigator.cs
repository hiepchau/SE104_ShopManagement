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
            Window oldWindow = Application.Current.MainWindow;
            Window newWindow = _windowProvider;
            newWindow.Show();
            Application.Current.MainWindow = newWindow;
            oldWindow?.Close();
        }
    }
}
