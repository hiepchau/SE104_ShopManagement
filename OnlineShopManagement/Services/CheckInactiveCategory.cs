using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.Services
{
    public static class CheckInactiveCategory
    {
        public async static Task<List<ProductTypeInfomation>> listInactiveCategory(MongoClient client, AppSession session, ProductsInformation pro)
        {
            FilterDefinition<ProductTypeInfomation> fil = Builders<ProductTypeInfomation>.Filter.Eq(x => x.ID, pro.Category) &
                   Builders<ProductTypeInfomation>.Filter.Eq(x => x.isActivated, false);
            List<ProductTypeInfomation> lscheck = new List<ProductTypeInfomation>();
            GetProductType gettercheck = new GetProductType(client, session, fil);
            lscheck = await gettercheck.Get();
            return lscheck;
        }
    }
}
