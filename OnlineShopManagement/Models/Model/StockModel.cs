using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class StockModel : BaseModel, IModel<StockInformation>
    {
        public StockModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<StockInformation>> GetEntity(FilterDefinition<StockInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<StockInformation>("StockingInformation");
            var field = Builders<StockInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.StockDay)
                .Include(p => p.User)
                .Include(p => p.producer)
                .Include(p => p.total)
                .Include(p=>p.displayID);

            var au = await collection.Find<StockInformation>(filter).Project<StockInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(StockInformation registob)
        {
            var stock = registob as StockInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("StockingInformation");
            var filtercheck = Builders<StockInformation>.Filter.Eq(x => x.displayID, stock.displayID) | Builders<StockInformation>.Filter.Eq(x => x.displayID, stock.ID);
            var task1 = GetEntity(filtercheck);
            var lscheck = await task1;
            Task.WaitAll(task1);
            if (string.IsNullOrEmpty(stock.ID) && string.IsNullOrEmpty(stock.displayID))
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (string.IsNullOrEmpty(stock.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument
            {
                {"StockDay",stock.StockDay},
                {"UserID",stock.User},
                {"ProducerID", stock.producer },
                {"Total", stock.total },
                {"DisplayID",stock.ID},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return (false, e.Message);
                }
            }
            else
            {
                BsonDocument newProductDoc = new BsonDocument
            {
                {"StockDay",stock.StockDay},
                {"UserID",stock.User},
                {"ProducerID", stock.producer },
                {"Total", stock.total },
                {"DisplayID",stock.displayID},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return(false, e.Message);
                }
            }
        }

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<StockInformation> filter, UpdateDefinition<StockInformation> updatedata)
        {
            throw new NotImplementedException();
        }
    }
}
