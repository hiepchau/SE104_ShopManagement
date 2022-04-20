using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsTypeFunction:BaseFunction
    {
        public List<ProductTypeInfomation> listItemsProductType { get; set; }
        public ProductsTypeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            listItemsProductType = new List<ProductTypeInfomation>();
            listItemsProductType.Add(new ProductTypeInfomation("1", "Nuoc giai khat"));
        }
    }
}
