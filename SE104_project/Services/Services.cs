using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Services.Common;

namespace SE104_OnlineShopManagement.Services
{
    public static class Services
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<IViewState, ViewState>();
            });
            return host;
        }
        public static IHostBuilder AddModels(this IHostBuilder host)
        {
            return null;
        }

        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return null;
        }

        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            return null; 
        }
    }
}
