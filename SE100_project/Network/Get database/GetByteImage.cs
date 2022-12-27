using MongoDB.Driver;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetByteImage
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ByteImage> _filter;
        public GetByteImage(MongoClient client, AppSession session, FilterDefinition<ByteImage> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }

        public async Task<List<ByteImage>> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ByteImage>("Images");
            var field = Builders<ByteImage>.Projection
                .Include(p => p.obID)
                .Include(p => p.data);
            var ls = await collection.Find<ByteImage>(_filter).Project<ByteImage>(field).ToListAsync();
            return ls;
        }
    }
}
