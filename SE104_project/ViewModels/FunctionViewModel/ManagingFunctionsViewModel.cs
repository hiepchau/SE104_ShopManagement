using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
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
            //set menu acording to menu name
        }

    }
}
