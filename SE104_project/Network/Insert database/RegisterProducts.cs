using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("ProductsInformation");
            BsonDocument newProductDoc = new BsonDocument{
                {"ProductName", newProduct.name },
                {"ProductQuantity", newProduct.quantity},
                {"ProductPrice", newProduct.price},
                {"ProductStockCost", newProduct.StockCost},
                {"ProductCategory", newProduct.Category},
                {"ProductProvider", newProduct.ProducerInformation },
            };
            collection.InsertOne(newProductDoc);
            Console.WriteLine("User Inserted into", session.CurrnetUser);
        }
    }
}

