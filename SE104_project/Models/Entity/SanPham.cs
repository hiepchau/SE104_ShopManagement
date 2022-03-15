using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class SanPham
    {
        public string MaSP { get; private set; }   
        public string TenSP { get; private set; }
        public string SL { get; private set; }
        public string NCC { get; private set; }
        public int Gia { get; private set; }

        public SanPham(string maSP, string tenSP, string sL, string nCC, int gia)
        {
            MaSP = maSP;
            TenSP = tenSP;
            SL = sL;
            NCC = nCC;
            Gia = gia;
        }
    }
}
