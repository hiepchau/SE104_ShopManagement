using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    internal class ThongTinNCC
    {
        public string IDNCC { get;private set; }
        public string TenNCC { get;private set; }
        public string PhoneNumber { get;private set; }
        public int TongTienDaNhap { get;private set; }

        public ThongTinNCC(string iDNCC, string tenNCC, string phoneNumber, int tongTienDaNhap)
        {
            IDNCC = iDNCC;
            TenNCC = tenNCC;
            PhoneNumber = phoneNumber;
            TongTienDaNhap = tongTienDaNhap;
        }
    }
}
