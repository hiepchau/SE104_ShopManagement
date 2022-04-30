using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MongoDB.Driver;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsTypeFunction:BaseFunction
    {
        #region Properties
        public string productTypeName { get; set; }
        public string note { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<ProductTypeInfomation> listItemsProductType { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        
        public ProductsTypeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            listItemsProductType = new ObservableCollection<ProductTypeInfomation>();
            GetData();
            listItemsProductType.Add(new ProductTypeInfomation("1", "Nuoc giai khat"));
            SaveCommand = new RelayCommand<Object>(null, SaveProductType);
        }
        public async void SaveProductType(object o = null)
        {
            if (CheckExist()==false)
            {
                ProductTypeInfomation info = new ProductTypeInfomation("", productTypeName, note);
                RegisterProductType regist = new RegisterProductType(info, _connection.client, _session);
                string s = await regist.register();
                listItemsProductType.Add(info);
                OnPropertyChanged(nameof(listItemsProductType));
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine("Cant insert because ProductTypeName has existed!");
            }
        }
        public async void GetData()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Empty;
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation pro in ls)
            {
                listItemsProductType.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsProductType));
        }
        public bool CheckExist()
        {
            foreach (ProductTypeInfomation pro in listItemsProductType)
            {
                if (productTypeName == pro.name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
