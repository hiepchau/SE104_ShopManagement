using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    public class RegisterProducts
    {
        private ProductsInformation newProduct;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterProducts(ProductsInformation newProduct, MongoClient client, AppSession ses)
        {
            this.newProduct = newProduct;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductsInformation");
            var projectioncheck = Builders<BsonDocument>.Projection.Include("DisplayID");
            var filtercheck = Builders<BsonDocument>.Filter.Eq("DisplayID", newProduct.ID);
            var lscheck = await collection.Find(filtercheck).Project(projectioncheck).ToListAsync();
            if(string.IsNullOrEmpty(newProduct.ID) && string.IsNullOrEmpty(newProduct.displayID))
            {
                Console.WriteLine("Insert error!");
                return null;
            }
            if (lscheck.Count > 0)
            {
                Console.WriteLine("Insert error");
                return null;
            }
            if (string.IsNullOrEmpty(newProduct.displayID)) { 
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
            await collection.InsertOneAsync(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser);
            return newProductDoc["_id"].ToString();
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
            };
                await collection.InsertOneAsync(newProductDoc);
                Console.WriteLine("User Inserted into", session.CurrnetUser);
                return newProductDoc["_id"].ToString();
            }
        }
    }
}

