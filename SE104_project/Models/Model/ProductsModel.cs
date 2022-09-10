using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    public class ProductsModel : BaseModel, IModel<ProductsInformation>
    {
        public ProductsModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<ProductsInformation>> GetEntity(FilterDefinition<ProductsInformation> filter)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductsInformation>("ProductsInformation");
            var field = Builders<ProductsInformation>.Projection
                .Include(p => p.ID)
                .Include(p => p.StockCost)
                .Include(p => p.quantity)
                .Include(p => p.price)
                .Include(p => p.ProducerInformation)
                .Include(p => p.Category)
                .Include(p => p.name)
                .Include(p => p.Unit)
                .Include(p => p.displayID)
                .Include(p => p.isActivated);

            var au = await collection.Find<ProductsInformation>(filter).Project<ProductsInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(ProductsInformation registob)
        {
            var newProduct = registob as ProductsInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductsInformation");
            var filtercheck = Builders<ProductsInformation>.Filter.Eq(x => x.displayID, newProduct.displayID) | Builders<ProductsInformation>.Filter.Eq(x => x.displayID, newProduct.ID);
            var task1 = GetEntity(filtercheck);
            var lscheck = await task1;
            if (string.IsNullOrEmpty(newProduct.ID) && string.IsNullOrEmpty(newProduct.displayID))
            {
                Console.WriteLine("Insert error!");
                return (false,null);
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return (false,null);
            }
            if (string.IsNullOrEmpty(newProduct.displayID))
            {
                BsonDocument newProductDoc = new BsonDocument{
                {"ProductName", newProduct.name },
                {"ProductQuantity", newProduct.quantity},
                {"ProductPrice", newProduct.price},
                {"ProductStockCost", newProduct.StockCost},
                {"ProductCategory", newProduct.Category},
                {"ProductProvider", newProduct.ProducerInformation },
                {"Unit", newProduct.Unit },
                {"DisplayID",newProduct.ID},
                {"isActivated",true},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser);
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
                {"ProductName", newProduct.name },
                {"ProductQuantity", newProduct.quantity},
                {"ProductPrice", newProduct.price},
                {"ProductStockCost", newProduct.StockCost},
                {"ProductCategory", newProduct.Category},
                {"ProductProvider", newProduct.ProducerInformation },
                {"Unit", newProduct.Unit },
                {"DisplayID",newProduct.displayID},
                {"isActivated",newProduct.isActivated},
            };
                try
                {
                    await collection.InsertOneAsync(newProductDoc);
                    Console.WriteLine("User Inserted into", _session.CurrnetUser);
                    return (true,newProductDoc["_id"].ToString());
                }
                catch(Exception e)
                {
                    return(false,e.Message);
                }
            }
        }

        public async Task<(bool isSuccessful, string message)> Update(FilterDefinition<ProductsInformation> filter, UpdateDefinition<ProductsInformation> updatedata)
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductsInformation>("ProductsInformation");
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
