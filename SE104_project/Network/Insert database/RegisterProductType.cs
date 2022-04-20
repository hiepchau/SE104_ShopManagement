using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    class RegisterProductType
    {
        private ProductTypeInfomation newShip;
        private MongoClient mongoClient;
        private AppSession session;

        public RegisterProductType(ProductTypeInfomation info, MongoClient client, AppSession session)
        {
            this.newShip = info;
            this.mongoClient = client;
            this.session = session;
        }

        public string register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductTypeInfomation");
            BsonDocument newProductDoc = new BsonDocument
            {
                {"ProductTypeName", newShip.name },
                {"Note", newShip.note }
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("ProductType Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}
