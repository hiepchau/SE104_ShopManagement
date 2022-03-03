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
        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider { get=>_serviceProvider;set=>_serviceProvider = value; }
        public App()
        {
            Instance = this;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
        }
    }
}
