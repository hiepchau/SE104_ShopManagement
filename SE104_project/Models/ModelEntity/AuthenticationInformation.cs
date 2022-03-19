using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class AuthenticationInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public AuthenticationInformation(string User, string pass)
        {
            UserName = User;    
            Password = pass;
        }
    }
}
