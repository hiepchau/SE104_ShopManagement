using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class CustomerModel : BaseModel, IModel<CustomerInformation>
    {
        public CustomerModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<CustomerInformation>> GetEntity(FilterDefinition<CustomerInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<CustomerInformation>("CustomerInformation");
            var field = Builders<CustomerInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.Name)
                .Include(p => p.PhoneNumber)
                .Include(p => p.CMND)
                .Include(p => p.Address)
                .Include(p => p.CustomerLevel)
                .Include(p => p.displayID)
                .Include(p => p.isActivated);

            var au = await collection.Find<CustomerInformation>(filter).Project<CustomerInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(CustomerInformation registob)
        {
            var customer = registob as CustomerInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("CustomerInformation");
            var filtercheck = Builders<CustomerInformation>.Filter.Eq(x => x.displayID, customer.displayID) | Builders<CustomerInformation>.Filter.Eq(x => x.displayID, customer.ID);
            var task1 = GetEntity(filtercheck);
            var lscheck = await task1;
            Task.WaitAll(task1);
            if (string.IsNullOrEmpty(customer.ID) && string.IsNullOrEmpty(customer.displayID))
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return (false,null);
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

                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into " + _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return (false, e.Message);
                }
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
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into " + _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return(false,e.Message);
                }
            }
        }

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<CustomerInformation> filter, UpdateDefinition<CustomerInformation> updatedata)
        {
            throw new NotImplementedException();
        }
    }
}
