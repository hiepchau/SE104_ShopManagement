using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            return Host.CreateDefaultBuilder(args);

        }
        public App()
        {
            Instance = this;
            _host= hostBuilder().Build();
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //_host.Services.GetRequiredService<>;
        }
    }
}
