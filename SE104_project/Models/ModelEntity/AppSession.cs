using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class AppSession
    {
        public string CurrentSession { get; set; }  
        public AppSession(string s = null)
        {
            CurrentSession = s;
        }
    }
}
