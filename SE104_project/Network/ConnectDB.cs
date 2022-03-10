using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
namespace SE104_OnlineShopManagement.Network
{
    public class ConnectDB
    {
        FirestoreDb db;
        public void InitilizeDB()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Network\FirestoreDB.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create("do-an-82485");
            Console.WriteLine("Connect Database successfully");
        }
    }
}
