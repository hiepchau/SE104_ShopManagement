using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class BillModel : BaseModel, IModel<BillInformation>
    {
        public BillModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<BillInformation>> GetEntity(FilterDefinition<BillInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BillInformation>("BillsInformation");
            var field = Builders<BillInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.User)
                .Include(p => p.saleDay)
                .Include(p => p.customer)
                .Include(p => p.total)
                .Include(p => p.displayID);


            var au = await collection.Find<BillInformation>(filter).Project<BillInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(BillInformation registob)
        {
            var newBill = registob as BillInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("BillsInformation");
            var filtercheck = Builders<BillInformation>.Filter.Eq(x => x.displayID, newBill.displayID) | Builders<BillInformation>.Filter.Eq(x => x.displayID, newBill.ID);
            var task1 = GetEntity(filtercheck);
            var lscheck = await task1;
            Task.WaitAll(task1);
            if (string.IsNullOrEmpty(newBill.displayID) && string.IsNullOrEmpty(newBill.ID))
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (string.IsNullOrEmpty(newBill.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total},
                {"DisplayID",newBill.ID},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception ex)
                {
                    return (false,ex.Message);
                }
            }
            else
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"SaleDay", newBill.saleDay},
                {"User", newBill.User },
                {"Customer", newBill.customer },
                {"Total" , newBill.total},
                {"DisplayID",newBill.displayID},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser.companyInformation);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return(false,e.Message);
                }
            }
        }

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<BillInformation> filter)
        {
            throw new NotImplementedException();
        }
    }
}
