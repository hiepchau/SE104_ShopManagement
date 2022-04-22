using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Network.Insert_database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsTypeFunction:BaseFunction
    {
        public string productTypeName { get; set; }
        public string note { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #region
        public ICommand SaveCommand { get; set; }
        #endregion
        public List<ProductTypeInfomation> listItemsProductType { get; set; }
        public ProductsTypeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            listItemsProductType = new List<ProductTypeInfomation>();
            listItemsProductType.Add(new ProductTypeInfomation("1", "Nuoc giai khat"));
            SaveCommand = new RelayCommand<Object>(null, SaveProductType);
        }
        public async void SaveProductType(object o = null)
        {
            ProductTypeInfomation info = new ProductTypeInfomation("",productTypeName,note);
            RegisterProductType regist = new RegisterProductType(info, _connection.client, _session);
            string s = await regist.register();
            Console.WriteLine(s);
        }
    }
}
