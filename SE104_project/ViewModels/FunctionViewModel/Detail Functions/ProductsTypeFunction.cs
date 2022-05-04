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
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateProductTypeList
    {
        void UpdateProductTypeList(ProductTypeInfomation type);
    }
    class ProductsTypeFunction:BaseFunction, IUpdateProductTypeList
    {
        #region Properties
        public string productTypeName { get; set; }
        public string note { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<ProductsTypeControlViewModel> listItemsProductType { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        #endregion
        
        public ProductsTypeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            listItemsProductType = new ObservableCollection<ProductsTypeControlViewModel>();
            GetData();
            listItemsProductType.Add(new ProductsTypeControlViewModel(new ProductTypeInfomation("1", "Nuoc giai khat"), this));
            SaveCommand = new RelayCommand<Object>(null, SaveProductType);
        }
        #region Function
        public async void SaveProductType(object o = null)
        {
            if (CheckExist()==false)
            {
                ProductTypeInfomation info = new ProductTypeInfomation("", productTypeName, note);
                RegisterProductType regist = new RegisterProductType(info, _connection.client, _session);
                string s = await regist.register();
                info.ID = s;
                listItemsProductType.Add(new ProductsTypeControlViewModel(info,this));
                OnPropertyChanged(nameof(listItemsProductType));
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine("ProductTypeName has existed!");
            }
        }
        public void UpdateProductTypeList(ProductTypeInfomation type)
        {
            int i = 0;
            if (listItemsProductType.Count > 0)
            {
                foreach (ProductsTypeControlViewModel ls in listItemsProductType)
                {
                    if (ls.type.Equals(type))
                    {
                        break;
                    }
                    i++;
                }
                listItemsProductType.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsProductType));
            }
            else
            {
                return;
            }
        }

        #endregion

        #region DB
        public async void GetData()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Empty;
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation type in ls)
            {
                listItemsProductType.Add(new ProductsTypeControlViewModel(type, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsProductType));
        }
        public bool CheckExist()
        {
            foreach (ProductsTypeControlViewModel type in listItemsProductType)
            {
                if (productTypeName == type.name)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
