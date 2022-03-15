using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    public class KhoHang
    {
        public string MaSP { get;private set; }
        public int Amount { get;private set;}
        public string Name { get;private set; }
        public int VonTonKho { get;private set; } 
        public int GiaTriTon { get;private set; }

        public KhoHang(string maSP, int amount, string name, int vonTonKho, int giaTriTon)
        {
            MaSP = maSP;
            Amount = amount;
            Name = name;
            VonTonKho = vonTonKho;
            GiaTriTon = giaTriTon;
        }
    }
}
