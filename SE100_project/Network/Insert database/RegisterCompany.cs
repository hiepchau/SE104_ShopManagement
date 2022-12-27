using MongoDB.Bson;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Network.Insert_database
{
    class RegisterCompany
    {
        private CompanyInformation company;
        private MongoClient mongoClient;
        private AppSession session;
        public RegisterCompany(CompanyInformation company, MongoClient client, AppSession ses)
        {
            this.company = company;
            this.mongoClient = client;
            this.session = ses;
        }
        public async Task<string> register()
        {
            var database = mongoClient.GetDatabase(session.CurrnetUser.companyInformation);
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
            await collection.InsertOneAsync(newCompanyDoc);
            Console.WriteLine("Company Inserted into " + session.CurrnetUser.companyInformation);
            return newCompanyDoc["_id"].ToString();
        }
    }
}
