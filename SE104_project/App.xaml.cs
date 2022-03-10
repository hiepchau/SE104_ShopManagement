using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.ViewModels;
using SE104_OnlineShopManagement.ViewModels.Authentication;
using SE104_OnlineShopManagement.Views.Windows;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SE104_project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        IHost _host;
        public static IHostBuilder hostBuilder(string[] args =null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddStores()
                .AddViewModels()
                .AddViews();

        }
        public App()
        {
            Instance = this;
            _host= hostBuilder().Build();
            
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConnectDB connect = new ConnectDB();
            connect.InitilizeDB();
            //IServiceCollection services = new ServiceCollection();
            //services.AddTransient<ViewModelBase>();
            //services.AddSingleton<LoginViewModel>();
            //services.AddSingleton<RegisterViewModel>();
            //services.AddSingleton<ViewModelFactory>();
            //services.AddSingleton<IViewState, ViewState>();
            //services.AddSingleton<MainViewModel>(s => new MainViewModel(s.GetRequiredService<IViewState>(), s.GetRequiredService<ViewModelFactory>())
            //{
            //    CurrentMainViewModel = s.GetRequiredService<LoginViewModel>()
            //}); ;
            //services.AddSingleton<AuthenticationWindow>(s => new AuthenticationWindow()
            //{
            //    DataContext = s.GetRequiredService<MainViewModel>()
            //}); ; ;
            //services.AddSingleton<MainWindowNavigator<AuthenticationWindow>>();
            //var serviceprovider = services.BuildServiceProvider();
            Language.ApplyLanguage("vi-VN");
            _host.Services.GetRequiredService<IViewModelFactory>().CreateViewModel<MainViewModel>().CurrentMainViewModel = _host.Services.GetRequiredService<LoginViewModel>();
            _host.Services.GetRequiredService<MainWindowNavigator<AuthenticationWindow>>().Navigate();
        }
    }
}
