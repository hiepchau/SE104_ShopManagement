using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterProducer
    {
        private ProducerInformation producer;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterProducer(ProducerInformation newcus, MongoClient client, AppSession ses)
        {
            this.producer = newcus;
            this.mongoClient = client;
            this.session = ses;
        }
        public string register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProducerInfromation");
            BsonDocument newProductDoc = new BsonDocument{
                {"Name", producer.Name},
                {"Email", producer.Email },
                {"Phone", producer.PhoneNumber },
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("Object Inserted into " + session.CurrnetUser.companyInformation);
            return newProductDoc["_id"].ToString();
        }
    }
}
