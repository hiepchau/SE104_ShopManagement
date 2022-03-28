using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterCustomer
    {
        private CustomerInformation customer;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterCustomer(CustomerInformation newcus, MongoClient client, AppSession ses)
        {
            this.customer = newcus;
            this.mongoClient = client;
            this.session = ses;
        }
        public void register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("CustomerInformation");
            BsonDocument newProductDoc = new BsonDocument{
                {"Name", customer.Name},
                {"Email", customer.Email },
                {"Phone", customer.PhoneNumber },
                {"Level", customer.CustomerLevel },
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
        }
    }
}
