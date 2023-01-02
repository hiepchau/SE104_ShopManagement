using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Utils
{
    public static class RoleSeperator
    {
        public static bool managerRole(AppSession session)
        {
            if ((int)session.CurrnetUser.role <= 1) { 
                return true;
            }
            return false;
        }

        public static bool ownerloyeeRole(AppSession session)
        {
            if ((int)session.CurrnetUser.role == 0)
            {
                return true;
            }
            return false;
        }
    }
}
