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
        public void SaveProductType(object parameter)
        {
            var values=(object[])parameter;
            ProductTypeInfomation info = new ProductTypeInfomation("",values[0].ToString(),values[1].ToString());
            RegisterProductType regist = new RegisterProductType(info, _connection.client, _session);
            string s = regist.register();
            Console.WriteLine(s);
        }
    }
}
