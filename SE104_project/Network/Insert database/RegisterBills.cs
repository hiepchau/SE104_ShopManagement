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
            var projectioncheck = Builders<BsonDocument>.Projection.Include("DisplayID");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("DisplayID", newBill.displayID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if(string.IsNullOrEmpty(newBill.displayID) && string.IsNullOrEmpty(newBill.ID))
            {
                Console.WriteLine("Insert error");
                return null;
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return null;
            }
            if (string.IsNullOrEmpty(newBill.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total},
                {"DisplayID",newBill.ID},
            };
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
                return newProductDoc["_id"].ToString();
            }
            else
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total},
                {"DisplayID",newBill.displayID},
            };
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
                return newProductDoc["_id"].ToString();
            }
        }
    }
}

