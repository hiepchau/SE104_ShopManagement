using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterBills
    {
        private BillInformation newBill;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterBills(BillInformation newbill, MongoClient client, AppSession ses)
        {
            this.newBill = newbill;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("BillsInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("_id");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("_id", newBill.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return null;
            }
            BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total}
            };
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}

