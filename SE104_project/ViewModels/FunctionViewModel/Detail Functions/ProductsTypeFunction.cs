using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.Network.Update_database;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MongoDB.Driver;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System.Windows;
using SE104_OnlineShopManagement.Services;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateProductTypeList
    {
        void UpdateProductTypeList(ProductTypeInfomation type);
        void EditProductType(ProductTypeInfomation type);
    }
    class ProductsTypeFunction:BaseFunction, IUpdateProductTypeList
    {
        #region Properties
        public string productTypeName { get; set; }
        public string note { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ProductsTypeControlViewModel selectedProductType { get; set; }
        public ObservableCollection<ProductsTypeControlViewModel> listItemsProductType { get; set; }
        public ObservableCollection<ProductsTypeControlViewModel> listItemsUnactiveProductType { get; set; }
        #endregion
        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand SetUnactiveCommand { get; set; }
        public ICommand SetActiveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        public ProductsTypeFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            listItemsProductType = new ObservableCollection<ProductsTypeControlViewModel>();
            listItemsUnactiveProductType = new ObservableCollection<ProductsTypeControlViewModel>();
            //Get Data
            GetData();
            GetUnactiveProductType();
            //
            SaveCommand = new RelayCommand<Object>(null, SaveProductType);
            SetUnactiveCommand = new RelayCommand<Object>(null, SetUnactive);
            SetActiveCommand = new RelayCommand<Object>(null, SetActive);
            CancelCommand = new RelayCommand<Object>(null, SetNull);
        }
        #region Function
        public void SetNull(object o = null)
        {
            selectedProductType = null;
            productTypeName = "";
            note = "";
            OnPropertyChanged(nameof(productTypeName));
            OnPropertyChanged(nameof(note));
        }
        public async void SaveProductType(object o = null)
        {
            if (selectedProductType!=null)
            {
                var filter = Builders<ProductTypeInfomation>.Filter.Eq("ID", selectedProductType.ID);
                var update = Builders<ProductTypeInfomation>.Update.Set("ProductTypeName", productTypeName).Set("Note", note);
                UpdateProductTypeInformation updater = new UpdateProductTypeInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listItemsProductType.Clear();
                GetData();
                OnPropertyChanged(nameof(listItemsProductType));
            }
            else if (CheckExist()==false)
            {
                ProductTypeInfomation info = new ProductTypeInfomation("", productTypeName, note);
                RegisterProductType regist = new RegisterProductType(info, _connection.client, _session);
                string s = await regist.register();
                info.ID = s;
                listItemsProductType.Clear();
                GetData();
                OnPropertyChanged(nameof(listItemsProductType));
                Console.WriteLine(s);
            }
            else
            {
                Console.WriteLine("ProductTypeName has existed!");
            }
            //Set Null
            SetNull();
        }
        public void UpdateProductTypeList(ProductTypeInfomation type)
        {
            var result = CustomMessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
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
            else
            {
                CustomMessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        public async void SetUnactive(object o = null)
        {
            if (selectedProductType != null && selectedProductType.isActivated == true)
            {
                var filter = Builders<ProductTypeInfomation>.Filter.Eq("ID", selectedProductType.ID);
                var update = Builders<ProductTypeInfomation>.Update.Set("isActivated", false);
                UpdateProductTypeInformation updater = new UpdateProductTypeInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listItemsUnactiveProductType.Clear();
                listItemsProductType.Clear();
                GetData();
                GetUnactiveProductType();
                OnPropertyChanged(nameof(listItemsUnactiveProductType));
                OnPropertyChanged(nameof(listItemsProductType));
                Console.WriteLine(s);
                selectedProductType = null;
            }
            else Console.WriteLine("Cant execute");
        }
        public async void SetActive(object o = null)
        {
            if (selectedProductType != null && selectedProductType.isActivated == false)
            {
                var filter = Builders<ProductTypeInfomation>.Filter.Eq("ID", selectedProductType.ID);
                var update = Builders<ProductTypeInfomation>.Update.Set("isActivated", true);
                UpdateProductTypeInformation updater = new UpdateProductTypeInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                listItemsUnactiveProductType.Clear();
                listItemsProductType.Clear();
                GetData();
                GetUnactiveProductType();
                OnPropertyChanged(nameof(listItemsUnactiveProductType));
                OnPropertyChanged(nameof(listItemsProductType));
                Console.WriteLine(s);
                selectedProductType = null;
            }
            else Console.WriteLine("Cant execute");
        }
        public void EditProductType(ProductTypeInfomation type)
        {
            if (listItemsProductType.Count > 0)
            {
                foreach (ProductsTypeControlViewModel ls in listItemsProductType)
                {
                    if (ls.type.Equals(type))
                    {
                        selectedProductType = ls;
                        break;
                    }
                }              
            }
            else
            {
                return;
            }
            productTypeName = selectedProductType.name;
            note = selectedProductType.note;
            OnPropertyChanged(nameof(productTypeName)); 
            OnPropertyChanged(nameof(note));           
        }
        #endregion

        #region DB
        public async void GetData()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq("isActivated",true);
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation type in ls)
            {
                listItemsProductType.Add(new ProductsTypeControlViewModel(type, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsProductType));
        }
        public async void GetUnactiveProductType()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq("isActivated", false);
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation type in ls)
            {
                listItemsUnactiveProductType.Add(new ProductsTypeControlViewModel(type, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsUnactiveProductType));
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
