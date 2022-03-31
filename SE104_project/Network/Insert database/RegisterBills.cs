using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("BillsInformation");
            BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total}
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser.companyInformation);
        }
    }
}

