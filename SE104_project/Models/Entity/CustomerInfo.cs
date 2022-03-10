using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;
namespace SE104_OnlineShopManagement.Models.Entity
{
    public class CustomerInfo
    {
        FirestoreDb db;
        public Guid Id { get; private set; }
        public string Name { get;private set; }      
        public DateTime Birthday { get;private set; }
        public string PhoneNumber { get;private set; }
        public DateTime LanCuoiMuahHang { get;private set; }
        public int TongTienDaMua { get;private set; }

        public CustomerInfo(Guid id, string name, DateTime lancuoimuahang, DateTime bday, string phone, int tongtien)
        {
            this.Id = id;
            this.Name = name;
            this.LanCuoiMuahHang = lancuoimuahang;
            this.Birthday = bday;
            this.PhoneNumber = phone;
            this.TongTienDaMua = tongtien;
        }
        public async void GetAllCustomerInfo()
        {
            Query qref = db.Collection("ThongTinKhachHang");
            QuerySnapshot snap = await qref.GetSnapshotAsync();
            foreach(DocumentSnapshot docsnap in snap)
            {
                CustomerInfo customerInfo=docsnap.ConvertTo<CustomerInfo>();
            }
        }
    }
}
