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
        public void InitilizeDB()
        {
            string path = @"Data Source=.\sqlexpress;Initial Catalog=QuanLyBanHang;Integrated Security=True";
            SqlConnection ketnoi=new SqlConnection(path);
            ketnoi.Open();
        }
    }
}
