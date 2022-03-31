using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel
{
    public class BaseFunction:ViewModelBase
    {
        protected AppSession Session { get; set; }
        protected MongoConnect Connect { get; set; }  
        public BaseFunction(AppSession session, MongoConnect connect)
        {
            this.Session = session; 
            this.Connect = connect;
        }
    }
}
