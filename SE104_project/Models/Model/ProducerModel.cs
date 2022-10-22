using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class ProducerModel : BaseModel, IModel<ProducerInformation>
    {
        public ProducerModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<ProducerInformation>> GetEntity(FilterDefinition<ProducerInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProducerInformation>("ProducerInformation");
            var field = Builders<ProducerInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.Name)
                .Include(p => p.Email)
                .Include(p => p.PhoneNumber)
                .Include(p => p.Address)
                .Include(p => p.isActivated)
                .Include(p => p.displayID);

            var au = await collection.Find<ProducerInformation>(filter).Project<ProducerInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(ProducerInformation registob)
        {
            var producer = registob as ProducerInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProducerInformation");
            var filtercheck = Builders<ProducerInformation>.Filter.Eq(x => x.displayID, producer.displayID) | Builders<ProducerInformation>.Filter.Eq(x => x.displayID, producer.ID);
            var task1 = GetEntity(filtercheck);
            var lscheck = await task1;
            Task.WaitAll(task1);
            if (string.IsNullOrEmpty(producer.ID) && string.IsNullOrEmpty(producer.displayID))
            {
                Console.WriteLine("Insert Error!");
                return (false,null);
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return (false, null);
            }
            if (string.IsNullOrEmpty(producer.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"Name", producer.Name},
                {"Email", producer.Email },
                {"Phone", producer.PhoneNumber },
                {"DisplayID",producer.ID},
                {"Address",producer.Address }
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("Object Inserted into " + _session.CurrnetUser.companyInformation);
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
                {"Name", producer.Name},
                {"Email", producer.Email },
                {"Phone", producer.PhoneNumber },
                {"DisplayID",producer.displayID},
                {"Address", producer.Address },
                {"isActivated",true},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("Object Inserted into " + _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return(false,e.Message);
                }
            }
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<ProducerInformation> filter, UpdateDefinition<ProducerInformation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProducerInformation>("ProducerInformation");
            try
            {
                object o = await collection.UpdateOneAsync(filter, updatedata);
                string s = o.ToString();
                return (true,s);
            }
            catch(Exception e)
            {
                return (false,e.Message);
            }
        }
    }
}
