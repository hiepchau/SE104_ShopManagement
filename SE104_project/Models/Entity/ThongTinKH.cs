using System;
using System.Collections.Generic;
using System.Text;
namespace SE104_OnlineShopManagement.Models.Entity
{
    public class ThongTinKH
    {
        public string Id { get; private set; }
        public string Name { get;private set; }      
        public string PhoneNumber { get;private set; }
        public int TongTienDaMua { get;private set; }

        public ThongTinKH(string id, string name, string phone, int tongtien)
        {
            this.Id = id;
            this.Name = name;
            this.PhoneNumber = phone;
            this.TongTienDaMua = tongtien;
        }
    }
}
