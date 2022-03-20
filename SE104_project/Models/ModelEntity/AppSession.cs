using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.ModelEntity
{
    public class AppSession
    {
        public string CurrentSession { get; set; }

        public int role { get;  set; }
        public AppSession(string s = null, int role=-1)
        {
            this.CurrentSession = s;
            this.role = role;
        }
    }
}
