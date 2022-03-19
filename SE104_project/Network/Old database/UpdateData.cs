//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data.SqlClient;
//using SE104_OnlineShopManagement.Network;
//using SE104_OnlineShopManagement.Models.Entity;
//namespace SE104_OnlineShopManagement.Network
//{
//    internal class UpdateData
//    {
//        ConnectDB db = new ConnectDB();
//        public void UpdateDangNhapData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string IDNV="", password="";
//            lenh.CommandText = "Insert into DangNhap (IDNV,passwordtext) values (@IDNV,@passwordtext)";
//            lenh.Parameters.AddWithValue("@IDNV",IDNV);
//            lenh.Parameters.AddWithValue("@passwordtext", password);
//            db.Close();
//        }
//        public void UpdateDonHangData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string IDDH = "", IDKH = "", MaNVBH="", MaSP="";
//            DateTime NgayBan = DateTime.Now;
//            int SL = 0, TongTien = 0;
//            lenh.CommandText = "Insert into DonHang (IDDonHang ,IDKhachHang, MaNVBH,MaSP,SoLuong,NgayBan,TongTien) values (@IDDonHang,@IDKhachHang,@MaNVBH,@MaSP,@SoLuong,@NgayBan,@TongTien)";
//            lenh.Parameters.AddWithValue("@IDDonHang", IDDH);
//            lenh.Parameters.AddWithValue("@IDKhachHang", IDKH);
//            lenh.Parameters.AddWithValue("@MaNVBH",MaNVBH);
//            lenh.Parameters.AddWithValue("@MaSP", MaSP);
//            lenh.Parameters.AddWithValue("@NgayBan",NgayBan);
//            lenh.Parameters.AddWithValue("@SL", SL);
//            lenh.Parameters.AddWithValue("@TongTien",TongTien);
//            db.Close();
//        }
//        public void UpdateKhoHangData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string MaSP = "", TenSP = "";
//            int SL = 0, VonTonKho = 0, GiaTriTon = 0;
//            lenh.CommandText = "Insert into KhoHang (MaSP,TenSP,SoLuong,VonTonKho,GiaTriTon) values (@MaSP,@TenSP,@SoLuong,@VonTonKho,@GiaTriTon)";
//            lenh.Parameters.AddWithValue("@MaSP", MaSP);
//            lenh.Parameters.AddWithValue("@TenSP",TenSP);
//            lenh.Parameters.AddWithValue("@SoLuong", SL);
//            lenh.Parameters.AddWithValue("@VonTonKho",VonTonKho);
//            lenh.Parameters.AddWithValue("@GiaTriTon", GiaTriTon);
//            db.Close();
//        }
//        public void UpdatePhieuNhapData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string IDNhap = "", MaSP = "",IDNCC="",IDNVKho="";
//            int SL = 0, TongTien = 0;
//            DateTime NgayNhap = DateTime.Now;
//            lenh.CommandText = "Insert into PhieuNhap (IDNhap,MaSP,IDNCC,IDNVKho,SoLuong,NgayNhap,TongTien) values (@IDNhap,@MaSP,@IDNCC,@IDNVKho,@SoLuong,@NgayNhap,@TongTien)";
//            lenh.Parameters.AddWithValue("@IDNhap", IDNhap);
//            lenh.Parameters.AddWithValue("@MaSP", MaSP);
//            lenh.Parameters.AddWithValue("@IDNCC",IDNCC);
//            lenh.Parameters.AddWithValue("@IDNVKho",IDNVKho);
//            lenh.Parameters.AddWithValue("@SoLuong", SL);
//            lenh.Parameters.AddWithValue("@NgayNhap",NgayNhap);
//            lenh.Parameters.AddWithValue("@TongTien", TongTien);
//            db.Close();
//        }
//        public void UpdateSanPhamData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string TenSP = "", MaSP = "", TenNCC = "";
//            int SL = 0, Gia = 0;
//            lenh.CommandText = "Insert into DonHang (MaSP,TenSP,SoLuong,TenNCC,Gia) values (@MaSP,@TenSP,@SoLuong,@TenNCC,@Gia)";
//            lenh.Parameters.AddWithValue("@MaSP", MaSP);
//            lenh.Parameters.AddWithValue("@TenSP", TenSP);
//            lenh.Parameters.AddWithValue("@SoLuong", SL);
//            lenh.Parameters.AddWithValue("@TenNCC", TenNCC);
//            lenh.Parameters.AddWithValue("@Gia", Gia);
//            db.Close();
//        }
//        public void UpdateThongTinKHData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string TenKH = "", IDKH = "", SDT = "";
//            int TongTienDaMua = 0;
//            lenh.CommandText = "Insert into ThongTinKhachHang (TenKH,IDKhachHang,SDT,TongTienDaMua) values (@TenKH,@IDKhachHang,@SDT,@TongTienDaMua)";
//            lenh.Parameters.AddWithValue("@TenKH", TenKH);
//            lenh.Parameters.AddWithValue("@IDKhachHang", IDKH);
//            lenh.Parameters.AddWithValue("@SDT", SDT);
//            lenh.Parameters.AddWithValue("@TongTienDaMua", TongTienDaMua);
//            db.Close();
//        }
//        public void UpdateThongTinNCCData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string TenNCC = "", IDNCC = "", SDT = "";
//            int TongTienDaNhap = 0;
//            lenh.CommandText = "Insert into ThongTinNCC (TenNCC,IDNCC,SDT,TongTienDaNhap) values (@TenNCC,@IDNCC,@SDT,@TongTienDaNhap)";
//            lenh.Parameters.AddWithValue("@TenNCC", TenNCC);
//            lenh.Parameters.AddWithValue("@IDNCC", IDNCC);
//            lenh.Parameters.AddWithValue("@SDT", SDT);
//            lenh.Parameters.AddWithValue("@TongTienDaNhap", TongTienDaNhap);
//            db.Close();
//        }
//        public void UpdateThongTinNVData()
//        {
//            db.InitilizeDB();
//            SqlCommand lenh = new SqlCommand();
//            lenh.Connection = db.ketnoi;
//            string TenNV = "", IDNV = "", SDT = "",ChucNang="",TrangThai="";
//            lenh.CommandText = "Insert into ThongTinNV (TenNV,IDNV,SDT,ChucNang,TrangThai) values (@TenNV,@IDNV,@SDT,@ChucNang,@TrangThai)";
//            lenh.Parameters.AddWithValue("@TenNV", TenNV);
//            lenh.Parameters.AddWithValue("@IDNV", IDNV);
//            lenh.Parameters.AddWithValue("@SDT", SDT);
//            lenh.Parameters.AddWithValue("@ChucNang", ChucNang);
//            lenh.Parameters.AddWithValue("@TrangThai", TrangThai);
//            db.Close();
//        }
//    }
//}
