using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Services.Common;
using SE104_OnlineShopManagement.ViewModels.Authentication;
using SE104_OnlineShopManagement.Views.Windows;
using SE104_OnlineShopManagement.ViewModels;
using SE104_OnlineShopManagement.ViewModels.Home;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using MongoDB.Driver;

namespace SE104_OnlineShopManagement.Services
{
    public static class Services
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IViewState, ViewState>();
                services.AddSingleton<AppSession>();
            });
            return host;
        }

        public static IHostBuilder AddNetwork(this IHostBuilder host)
        {
            return host.ConfigureServices(services => { 
                services.AddSingleton<MongoConnect>();
            });
        }
        public static IHostBuilder AddModels(this IHostBuilder host)
        {
            return null;
        }

        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<SellingViewModel>();
                
                services.AddSingleton<ViewModelCreator<LoginViewModel>>(s => s.GetRequiredService<LoginViewModel>);
                services.AddSingleton<ViewModelCreator<RegisterViewModel>>(s => s.GetRequiredService<RegisterViewModel>);
                services.AddSingleton<ViewModelCreator<MainViewModel>>(s => s.GetRequiredService<MainViewModel>);
                services.AddSingleton<ViewModelCreator<HomeViewModel>>(s => s.GetRequiredService<HomeViewModel>);
                services.AddSingleton<ViewModelCreator<SellingViewModel>>(s=>s.GetRequiredService<SellingViewModel>);
                services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            });
            return host;
        }

        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            host.ConfigureServices(services => {
                services.AddTransient<AuthenticationWindow>(s => new AuthenticationWindow()
                {
                    DataContext = s.GetRequiredService<IViewModelFactory>().CreateViewModel<MainViewModel>()
                }); ; ;
                services.AddTransient<MainWindowNavigator<AuthenticationWindow>>();
              
                services.AddTransient<HomeWindow>(s => new HomeWindow()
                {
                DataContext = s.GetRequiredService<IViewModelFactory>().CreateViewModel<MainViewModel>()
                }); ; ;
                services.AddTransient<MainWindowNavigator<HomeWindow>>();
            });
            return host;
        }
    }
}
