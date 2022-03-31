using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions
{
    public class SellingViewModel:BaseFunction
    {
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
        }

    }
}
