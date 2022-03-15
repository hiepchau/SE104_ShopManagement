using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    internal class DangNhap
    {
        public string IDNV { get;private set; }
        public string password { get;private set; }

        public DangNhap(string iDNV, string password)
        {
            IDNV = iDNV;
            this.password = password;
        }
    }
}
