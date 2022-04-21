using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.MenuViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class ProductsFunction : BaseFunction
    {
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;
        public List<ProductsInformation> listItemsProduct { get; set; }

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
            listItemsProduct = new List<ProductsInformation>();
            //Test
            listItemsProduct.Add(new ProductsInformation("3", "Cocacola", 1, 10000, 5000, "Drink", "Cocacola", "lon"));
            //
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
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
        public async void SaveProduct(object parameter)
        {
            var values = (object[])parameter;
            long cost =long.Parse(values[3].ToString());
            long price = long.Parse(values[4].ToString());
            ProductsInformation info = new ProductsInformation("", values[0].ToString(), 1, price, cost,
                values[1].ToString(), "",values[2].ToString());
            RegisterProducts regist = new RegisterProducts(info, _connection.client, _session);
            string s = await regist.register();
            Console.WriteLine(s);
        }
    }
}
