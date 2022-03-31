using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterStocking
    {
        private StockInformation stock;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterStocking(StockInformation newstock, MongoClient client, AppSession ses)
        {
            this.stock = newstock;
            this.mongoClient = client;
            this.session = ses;
        }
        public void register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StockingInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"StockDay",stock.StockDay},
                {"UserID",stock.User},
                {"CustomerID", stock.customer },
                {"Total", stock.total }
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
        }
    }
}
