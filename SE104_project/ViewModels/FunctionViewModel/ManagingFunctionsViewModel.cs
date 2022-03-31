using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel
{
    public class ManagingFunctionsViewModel : BaseFunction
    {
        public ICommand changeDisplayCommand { get; set; }  

        public BaseFunction Currentdisplaying { get; set; }
        public MenuViewModel menuViewModel { get; set; }
        public ManagingFunctionsViewModel(AppSession session, MongoConnect connect) : base(session, connect)
        {
            
        }

    
        public void changeMenu(string MenuType)
        {
            if(MenuType == "Management")
            {
                menuViewModel = new ManagementMenu(Currentdisplaying,Session,Connect);
                OnPropertyChanged(nameof(menuViewModel));
            }
            if(MenuType == "Customer")
            {
                menuViewModel = new CustomerSelectMenu(Currentdisplaying,Session,Connect);
                OnPropertyChanged(nameof(menuViewModel));
            }
            if(MenuType == "Finance")
            {
                menuViewModel = new FinanceMenu(Currentdisplaying,Session,Connect);
                OnPropertyChanged(nameof(menuViewModel));
            }
            if(MenuType == "Report")
            {
                menuViewModel = new ReportMenu(Currentdisplaying,Session,Connect);
                OnPropertyChanged(nameof(menuViewModel));
            }
            if(MenuType == "Settings")
            {
                menuViewModel = new SettingMenu(Currentdisplaying,Session,Connect);
                OnPropertyChanged(nameof(menuViewModel));
            }

            
        }

    }
}
