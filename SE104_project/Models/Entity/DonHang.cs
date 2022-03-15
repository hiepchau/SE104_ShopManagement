using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    internal class DonHang
    {
        public string IDDonHang { get;private set; }
        public string IDKhachHang { get;private set; }
        public string MaNVBH { get;private set; }
        public string MaSP { get;private set; }
        public int SL { get;private set; }
        public DateTime NgayBan { get;private set; }
        public int TongTien { get;private set; }

        public DonHang(string iDDonHang, string iDKhachHang, string maNVBH, string maSP, int sL, DateTime ngayBan, int tongTien)
        {
            IDDonHang = iDDonHang;
            IDKhachHang = iDKhachHang;
            MaNVBH = maNVBH;
            MaSP = maSP;
            SL = sL;
            NgayBan = ngayBan;
            TongTien = tongTien;
        }
    }
}
