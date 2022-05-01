using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StockingInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", stock.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if (lscheck.Count > 0 || stock.ID=="")
            {
                Console.WriteLine("Insert error");
                return null;
            }
            BsonDocument newProductDoc = new BsonDocument
            {
                {"StockDay",stock.StockDay},
                {"UserID",stock.User},
                {"ProducerID", stock.producer },
                {"Total", stock.total }
            };
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}
