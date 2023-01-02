using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.Commands
{
    public static class StaticCommands
    {
        public static ICommand CloseWindowCommand = new RelayCommand<object>(
                null,
                parameter => { Window.GetWindow(Application.Current.MainWindow)?.Close(); });

        public static ICommand MaximizeWindowCommand = new RelayCommand<UserControl>(
                null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(Application.Current.MainWindow);
                    if (parentWindow != null)
                    {
                        parentWindow.WindowState = parentWindow.WindowState != WindowState.Maximized
                            ? WindowState.Maximized
                            : WindowState.Normal;
                    }
                });

        public static ICommand MinimizeWindowCommand = new RelayCommand<UserControl>(
               null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(Application.Current.MainWindow);
                    if (parentWindow != null)
                    {
                        parentWindow.WindowState = parentWindow.WindowState != WindowState.Minimized
                            ? WindowState.Minimized
                            : WindowState.Normal;
                    }
                });

        public static ICommand MouseMoveWindowCommand = new RelayCommand<UserControl>(
                null,
                parameter => Window.GetWindow(Application.Current.MainWindow)?.DragMove());
    }
}
