using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Models.Entity
{
    internal class ThongTinNV
    {
        public string IDNV { get;private set; }
        public string TenNV { get; private set; }
        public string PhoneNumber { get; private set; }
        public string ChucNang { get; private set; }
        public string TrangThai { get; private set; }

        public ThongTinNV(string iDNV, string tenNV, string phoneNumber, string chucNang, string trangThai)
        {
            IDNV = iDNV;
            TenNV = tenNV;
            PhoneNumber = phoneNumber;
            ChucNang = chucNang;
            TrangThai = trangThai;
        }
    }
}
