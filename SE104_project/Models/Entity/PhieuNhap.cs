using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    internal class PhieuNhap
    {
        public string IDNhap { get;private set; }
        public string MaSP { get;private set; }
        public string IDNCC { get;private set; }
        public string IDNVK { get;private set; }
        public int SL { get;private set; }
        public DateTime NgayNhap { get;private set; }
        public int TongTien { get;private set; }

        public PhieuNhap(string iDNhap, string maSP, string iDNCC, string iDNVK, int sL, DateTime ngayNhap, int tongTien)
        {
            IDNhap = iDNhap;
            MaSP = maSP;
            IDNCC = iDNCC;
            IDNVK = iDNVK;
            SL = sL;
            NgayNhap = ngayNhap;
            TongTien = tongTien;
        }
    }
}
