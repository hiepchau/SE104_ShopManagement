using SE104_OnlineShopManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.Commands
{
    public static class LanguageCommand
    {
        public static RoutedCommand ChangeLanguageCommand { get; set; }

        static LanguageCommand()
        {
            ChangeLanguageCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(ChangeLanguageCommand, LanguageChangeHandle, LanguageChangeCanExecute));
        }

        private static void LanguageChangeCanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            if (args.Command != ChangeLanguageCommand)
                return;
            args.CanExecute = true;
        }

        private static void LanguageChangeHandle(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Command != ChangeLanguageCommand)
                return;
            object Parameter = args.Parameter;
            Language.ApplyLanguage(Parameter.ToString());
        }

    }
}
