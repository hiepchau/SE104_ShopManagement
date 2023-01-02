using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    class GetProductType
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<ProductTypeInfomation> _filter;

        public GetProductType(MongoClient cl, AppSession ses, FilterDefinition<ProductTypeInfomation> fil)
        {
            _client = cl;
            _session = ses;
            _filter = fil;
        }

        public async Task<List<ProductTypeInfomation>> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductTypeInfomation>("ProductTypeInformation");
            var field = Builders<ProductTypeInfomation>.Projection
                .Include(x => x.ID)
                .Include(X => X.name)
                .Include(x => x.note)
                .Include(x=>x.isActivated);

            return await collection.Find<ProductTypeInfomation>(_filter).Project<ProductTypeInfomation>(field).ToListAsync();
        }
    }
}
