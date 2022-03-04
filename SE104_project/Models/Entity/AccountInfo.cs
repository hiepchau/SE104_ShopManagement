using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class AccountInfo
    {
        public string UserName { get;private set; }
        public string Password { get;private set; } 
        public AccountInfo(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
