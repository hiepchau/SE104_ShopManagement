using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class AuthenticationInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string companyName { get; set; }
        public AuthenticationInformation(string User, string pass, string companyname)
        {
            this.UserName = User;    
            this.Password = pass;
            this.companyName = companyname;
        }
    }
}
