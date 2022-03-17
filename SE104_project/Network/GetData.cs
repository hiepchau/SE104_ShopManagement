using System;
using System.Collections.Generic;
using System.Text;
using SE104_OnlineShopManagement.Network;
using System.Data.SqlClient;
using SE104_OnlineShopManagement.Models.Entity;
namespace SE104_OnlineShopManagement.Network
{
    internal class GetData
    {
        ConnectDB db = new ConnectDB();
        public void GetDangNhapData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from DangNhap";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var IDNV = reader["IDNV"];
                    var passwordtext = reader["Passwordtext"];
                    DangNhap dn = new DangNhap(IDNV.ToString(), passwordtext.ToString());
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetDonHangData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from DonHang";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var IDDH = reader["IDDonHang"];
                    var IDKH = reader["IDKhachHang"];
                    var MaNVBH = reader["MaNVBH"];
                    var MaSP = reader["MaSP"];
                    var SL = reader["SoLuong"];
                    var NgayBan = reader["NgayBan"];
                    var TongTien = reader["TongTien"];
                    int _sl=Int32.Parse(SL.ToString());
                    DateTime _ngayban=DateTime.Parse(NgayBan.ToString());
                    int _tongtien=Int32.Parse(TongTien.ToString());
                    DonHang dh = new DonHang(IDDH.ToString(),IDKH.ToString(),MaNVBH.ToString(),MaSP.ToString(),_sl,_ngayban,_tongtien);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetKhoHangData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from KhoHang";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {                   
                    var MaSP = reader["MaSP"];
                    var SL = reader["SoLuong"];
                    var TenSP = reader["TenSP"];
                    var VonTonKho = reader["VonTonKho"];
                    var GiaTriTon = reader["GiaTriTon"];
                    int _sl = Int32.Parse(SL.ToString());
                    int _vontonkho = Int32.Parse(VonTonKho.ToString());
                    int _giatriton = Int32.Parse(GiaTriTon.ToString());
                    KhoHang kh = new KhoHang(MaSP.ToString(),_sl,TenSP.ToString(),_vontonkho,_giatriton);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetPhieuNhapData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from PhieuNhap";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var IDNhap = reader["IDNhap"];
                    var MaSP = reader["MaSP"];
                    var IDNCC = reader["IDNCC"];
                    var IDNVKho = reader["IDNVKho"];
                    var SL = reader["SoLuong"];
                    var NgayNhap = reader["NgayNhap"];
                    var TongTien = reader["TongTien"];
                    int _sl = Int32.Parse(SL.ToString());
                    DateTime _ngaynhap = DateTime.Parse(NgayNhap.ToString());
                    int _tongtien = Int32.Parse(TongTien.ToString());
                    PhieuNhap pn = new PhieuNhap(IDNhap.ToString(), MaSP.ToString(), IDNCC.ToString(), IDNVKho.ToString(), _sl, _ngaynhap, _tongtien);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetSanPhamData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from SanPham";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var MaSP = reader["MaSP"];
                    var SL = reader["SoLuong"];
                    var TenSP = reader["TenSP"];
                    var TenNCC = reader["TenNCC"];
                    var Gia = reader["Gia"];
                    int _sl = Int32.Parse(SL.ToString());
                    int _gia = Int32.Parse(Gia.ToString());
                    SanPham sp = new SanPham(MaSP.ToString(), TenSP.ToString(), _sl, TenNCC.ToString(), _gia);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetThongTinKHData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from ThongTinKhachHang";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var TenKH = reader["TenKH"];
                    var IDKH = reader["IDKhachHang"];
                    var SDT = reader["SDT"];
                    var TongTienDaMua = reader["TongTienDaMua"];
                    int _tongtiendamua=Int32.Parse(TongTienDaMua.ToString());
                    ThongTinKH khachhang = new ThongTinKH(IDKH.ToString(),TenKH.ToString(), SDT.ToString(),_tongtiendamua);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetThongTinNCCData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from ThongTinNCC";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var IDNCC = reader["IDNCC"];
                    var TenNCC = reader["TenNCC"];
                    var SDT = reader["SDT"];
                    var TongTienDaNhap = reader["TongTienDaNhap"];
                    int _tongtiendanhap = Int32.Parse(TongTienDaNhap.ToString());
                    ThongTinNCC ncc = new ThongTinNCC(IDNCC.ToString(),TenNCC.ToString(),SDT.ToString(),_tongtiendanhap);
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
        public void GetThongTinNVData()
        {
            db.InitilizeDB();
            SqlCommand lenh = new SqlCommand();
            lenh.Connection = db.ketnoi;
            lenh.CommandText = "select * from ThongTinNV";
            var reader = lenh.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var IDNV = reader["IDNV"];
                    var TenNV = reader["TenNV"];
                    var SDT = reader["SDT"];
                    var ChucNang = reader["ChucNang"];
                    var TrangThai = reader["TrangThai"];
                    ThongTinNV nv = new ThongTinNV(IDNV.ToString(),TenNV.ToString(),SDT.ToString(),ChucNang.ToString(),TrangThai.ToString());
                }
            }
            else
            {
                Console.WriteLine("Khong co du lieu");
            }
            db.Close();
        }
    }
}
