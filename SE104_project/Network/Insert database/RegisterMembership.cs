using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    class RegisterMembership
    {
        private MembershipInformation newShip;
        private MongoClient mongoClient;
        private AppSession session;

        public RegisterMembership(MembershipInformation info, MongoClient client, AppSession session)
        {
            this.newShip = info;
            this.mongoClient = client;
            this.session = session;
        }

        public string register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("MembershipInformation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"MembershipName", newShip.name },
                {"Priority", newShip.priority }
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("Membership Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}
