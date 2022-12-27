using MongoDB.Driver;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Update_database
{
    public class UpdateImage
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ByteImage> _filter;
        private UpdateDefinition<ByteImage> _update;
        public UpdateImage(MongoClient cl, AppSession ses, FilterDefinition<ByteImage> filter, UpdateDefinition<ByteImage> update)
        {
            _client = cl;
            _session = ses;
            _filter = filter;
            _update = update;
        }
        public async Task<string> update()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ByteImage>("Images");
            object o = await collection.UpdateOneAsync(_filter, _update);
            string s = o.ToString();
            return s;
        }
    }
}
