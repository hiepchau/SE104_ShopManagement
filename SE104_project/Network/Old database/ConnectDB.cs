using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace SE104_OnlineShopManagement.Network
{
    public class ConnectDB
    {
        public SqlConnection ketnoi;
        public void InitilizeDB()
        {
            string path = @"Data Source=.\sqlexpress;Initial Catalog=QuanLyBanHang;Integrated Security=True";
            ketnoi=new SqlConnection(path);
            ketnoi.Open();
        }
        public void Close()
        {
            ketnoi.Close();
        }
    }
}
