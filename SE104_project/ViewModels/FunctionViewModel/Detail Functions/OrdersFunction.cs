﻿using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class OrdersFunction : BaseFunction
    {
        public OrdersFunction(AppSession session, MongoConnect connect) : base(session, connect)
        { 

        }
    }
}