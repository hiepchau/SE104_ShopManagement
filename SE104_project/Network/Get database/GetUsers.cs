using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Get_database
{
    public class GetUsers
    {
        private MongoClient _client;
        private AppSession _session;
        private FilterDefinition<UserInfomation> _filter;
        public GetUsers(MongoClient client, AppSession session, FilterDefinition<UserInfomation> filter)
        {
            _client = client;
            _session = session;
            _filter = filter;
        }

        public async Task<List<UserInfomation>> get()
        {
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<UserInfomation>("UserInformation");
            var field = Builders<UserInfomation>.Projection
                .Include(p => p.ID)
                .Include(p => p.FirstName)
                .Include(p => p.LastName)
                .Include(p => p.Email)
                .Include(p => p.Password)
                .Include(p => p.PhoneNumber)
                .Include(p => p.companyInformation)
                .Include(p => p.role)
                .Include(p => p.birthDay)
                .Include(p => p.salary)
                .Include(p => p.gender)
                .Include(p=>p.displayID)
                .Include(p=>p.isActivated)
                .Include(p=>p.workDate);
            var ls = await collection.Find<UserInfomation>(_filter).Project<UserInfomation>(field).ToListAsync();
            return ls;
        }
    }
}
