using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels
{
    public class TitleBarViewModel:ViewModelBase
    {
        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public TitleBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter => { Application.Current.Shutdown(); });

            MaximizeWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(parameter);
                    if (parentWindow != null)
                    {
                        parentWindow.WindowState = parentWindow.WindowState != WindowState.Maximized
                            ? WindowState.Maximized
                            : WindowState.Normal;
                    }
                });

            MinimizeWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(parameter);
                    if (parentWindow != null)
                    {
                        parentWindow.WindowState = parentWindow.WindowState != WindowState.Minimized
                            ? WindowState.Minimized
                            : WindowState.Normal;
                    }
                });

            MouseMoveWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter => Window.GetWindow(parameter)?.DragMove());
        }
    }
}
