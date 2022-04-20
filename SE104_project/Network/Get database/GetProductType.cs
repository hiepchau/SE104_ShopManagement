using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;

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

        public List<ProductTypeInfomation> Get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<ProductTypeInfomation>("ProductTypeInfomation");
            var field = Builders<ProductTypeInfomation>.Projection
                .Include(x => x.ID)
                .Include(X => X.name)
                .Include(x => x.note);

            return collection.Find<ProductTypeInfomation>(_filter).Project<ProductTypeInfomation>(field).ToList();
        }
    }
}
