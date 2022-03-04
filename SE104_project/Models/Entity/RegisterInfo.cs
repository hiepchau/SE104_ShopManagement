using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class RegisterInfo
    {
        public AccountInfo accountInfo { get; set; }
        public UserInfo userInfo { get; set; }
        RegisterInfo(AccountInfo acc, UserInfo user)
        {
            this.userInfo = user;
            this.accountInfo = acc;
        }
    }
}
