using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterBillDetails
    {
        private BillDetails newBill;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterBillDetails(BillDetails newbill, MongoClient client, AppSession ses)
        {
            this.newBill = newbill;
            this.mongoClient = client;
            this.session = ses;
        }
        public void register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("BillDetailsInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductID",newBill.productID},
                {"BillID", newBill.billID},
                {"Amount", newBill.amount},
                {"SumPrice", newBill.sumPrice},
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
        }
    }
}
