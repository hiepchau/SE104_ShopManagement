using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace SE104_OnlineShopManagement.Network
{
    public class MongoConnect
    {
        public MongoClient client { get; private set; }
        public MongoClientSettings settings { get; private set; }

        public MongoConnect()
        {
            settings = MongoClientSettings.FromConnectionString("mongodb+srv://DungTri:Zxcvbnm123@cluster0.1nc4v.mongodb.net/MyNewDatabase?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            this.client = new MongoClient(settings);
        }
    }
}
