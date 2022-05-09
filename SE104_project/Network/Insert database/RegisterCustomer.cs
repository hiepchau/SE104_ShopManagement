using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("CustomerInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("DisplayID");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("DisplayID", customer.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if(string.IsNullOrEmpty(customer.ID) && string.IsNullOrEmpty(customer.displayID))
            {
                Console.WriteLine("Insert error");
                return null;
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return null;
            }
            if (string.IsNullOrEmpty(customer.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"Name", customer.Name},
                {"Phone", customer.PhoneNumber },
                {"Level", customer.CustomerLevel },
                {"CMND", customer.CMND },
                {"Address", customer.Address },
                {"DisplayID",customer.ID},
                {"isActivated", customer.isActivated },
            };
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
                return newProductDoc["_id"].ToString();
            }
            else
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"Name", customer.Name},
                {"Phone", customer.PhoneNumber },
                {"Level", customer.CustomerLevel },
                {"CMND", customer.CMND },
                {"Address", customer.Address },
                {"DisplayID",customer.displayID},
                {"isActivated", customer.isActivated },
            };
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into " + session.CurrnetUser.companyInformation);
                return newProductDoc["_id"].ToString();
            }
        }
    }
}
