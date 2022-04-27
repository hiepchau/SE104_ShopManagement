using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MongoDB.Driver;
using System.Collections.ObjectModel;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsFunction : BaseFunction
    {
        #region properties
        public string productName { get; set; }
        public string productCategory { get; set; }
        public string productUnit { get; set; }
        public long productCost { get; set; }
        public long productPrice { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public ObservableCollection<ProductsInformation> listItemsProduct { get; set; }
        #endregion

        #region ICommand
        //Product
        public ICommand OpenAddProductControlCommand { get; set; }
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand OpenProductsTypeCommand { get; set; }
        //AddProduct
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion
        public ProductsFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            this._connection= connect;
            this._session= session;        
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            listItemsProduct = new ObservableCollection<ProductsInformation>();
            //Test
            GetData();
            listItemsProduct.Add(new ProductsInformation("3", "Cocacola", 1, 10000, 5000, "Drink", "Cocacola", "lon"));
            //
            OpenAddProductControlCommand = new RelayCommand<Object>(null, OpenAddProductControl);
            OpenProductsTypeCommand = new RelayCommand<Object>(null, OpenProductsType);
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);

        }
        public void OpenAddProductControl(Object o = null)
        {
            AddProductControl addProductControl = new AddProductControl();
            addProductControl.DataContext = this;
            DialogHost.Show(addProductControl);
            SaveCommand = new RelayCommand<Object>(null, SaveProduct);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
        public void OpenProductsType(Object o = null)
        {
            managingFunction.Currentdisplaying = new ProductsTypeFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(3);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        public void OpenImportProducts(Object o = null)
        {
            managingFunction.Currentdisplaying = new ImportProductsFunction(Session, Connect);
            ManagementMenu.changeSelectedItem(4);
            managingFunction.CurrentDisplayPropertyChanged();
        }
        public async void SaveProduct(object o = null)
        {
            ProductsInformation info = new ProductsInformation("", productName,1,productPrice,productCost,"","",productUnit);
            RegisterProducts regist = new RegisterProducts(info, _connection.client, _session);
            string s = await regist.register();
            listItemsProduct.Add(info);
            OnPropertyChanged(nameof(listItemsProduct));
            Console.WriteLine(s);
        }
        public async void GetData()
        {
            var filter = Builders<ProductsInformation>.Filter.Empty;
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listItemsProduct.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listItemsProduct));
        }
    }
}
