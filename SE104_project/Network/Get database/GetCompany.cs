using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    class GetCompany
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<CompanyInformation> _filter;
        public GetCompany(MongoClient client, AppSession session, FilterDefinition<CompanyInformation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }
        public async Task<List<CompanyInformation>> Get()
        {

            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<CompanyInformation>("CompanyInformation");
            var field = Builders<CompanyInformation>.Projection
                .Include(p => p.SessionID)
                .Include(p => p.Name)
                .Include(p => p.Address)
                .Include(p => p.Phone)
                .Include(p => p.Email)
                .Include(p => p.Facebook)
                .Include(p => p.Instagram)
                .Include(p => p.Website)
                .Include(p => p.TIN);
            var au = await collection.Find<CompanyInformation>(_filter).Project<CompanyInformation>(field).ToListAsync();
            return au;
        }
    }
}
