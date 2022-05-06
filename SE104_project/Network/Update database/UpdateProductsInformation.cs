using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Update_database
{
    public class UpdateProductsInformation
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ProductsInformation> _filter;
        private UpdateDefinition<ProductsInformation> _update;
        public UpdateProductsInformation(MongoClient cl, AppSession ses, FilterDefinition<ProductsInformation> filter, UpdateDefinition<ProductsInformation> update)
        {
            _client = cl;
            _session = ses;
            _filter = filter;
            _update = update;
        }
        public async Task<string> update()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductsInformation>("ProductsInformation");
            object o = await collection.UpdateOneAsync(_filter, _update);
            string s = o.ToString();
            return s;
        }
    }
}
