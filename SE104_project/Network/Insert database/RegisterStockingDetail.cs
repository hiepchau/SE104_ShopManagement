using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterStockingDetail
    {
        private StockDetails detail;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterStockingDetail(StockDetails detail, MongoClient client, AppSession ses)
        {
            this.detail = detail;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StockDetailInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductID",detail.productID},
                {"BillID", detail.stockID},
                {"Amount", detail.amount},
                {"SumPrice", detail.sumPrice},
            };
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}
