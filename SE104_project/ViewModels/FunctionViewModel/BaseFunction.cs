using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel
{
    public class BaseFunction:ViewModelBase
    {
        public AppSession Session { get; private set; }
        public MongoConnect Connect { get; private set; } 
        public BaseFunction(AppSession session, MongoConnect connect)
        {
            this.Session = session; 
            this.Connect = connect;
        }
    }
}
