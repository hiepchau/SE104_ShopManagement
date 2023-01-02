using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class AppSession
    {
        public UserInfomation CurrnetUser { get; set; }
        public AppSession(UserInfomation user=null)
        {
            this.CurrnetUser = user;
        }
    }
}
