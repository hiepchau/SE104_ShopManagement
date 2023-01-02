using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Models.Model
{
    internal class CompanyModel : BaseModel, IModel<CompanyInformation>
    {
        public CompanyModel(MongoClient client, AppSession session) : base(client, session)
        {
        }

        public async Task<List<CompanyInformation>> GetEntity(FilterDefinition<CompanyInformation> filter)
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
            var au = await collection.Find<CompanyInformation>(filter).Project<CompanyInformation>(field).ToListAsync();
            return au;
        }

        public async Task<(bool isSuccessful, string message)> Register(CompanyInformation registob)
        {
            var company = registob as CompanyInformation;
            var database = _client.GetDatabase(_session.CurrnetUser.companyInformation);
            var collection = database.GetCollection<BsonDocument>("CompanyInformation");
            BsonDocument newCompanyDoc = new BsonDocument
            {
                {"CompanyName", company.Name },
                {"CompanyAddress", company.Address },
                {"CompanyPhone", company.Phone },
                {"CompanyEmail", company.Email },
                {"CompanyFacebook", company.Facebook },
                {"CompanyInstagram", company.Instagram },
                {"CompanyTIN", company.TIN },
                {"CompanyWebsite", company.Website },

            };
            try
            {
                await collection.InsertOneAsync(newCompanyDoc);
                Console.WriteLine("Company Inserted into " + _session.CurrnetUser.companyInformation);
                return (true,newCompanyDoc["_id"].ToString());
            }
            catch(Exception e)
            {
                return (false, e.Message);
            }
            }
            

        public Task<(bool isSuccessful, string message)> Update(FilterDefinition<CompanyInformation> filter, UpdateDefinition<CompanyInformation> updatedata)
        {
            throw new NotImplementedException();
        }
    }
}
