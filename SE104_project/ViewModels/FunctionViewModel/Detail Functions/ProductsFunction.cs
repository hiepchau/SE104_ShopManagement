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
using SE104_OnlineShopManagement.Services;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System.Threading.Tasks;
using SE104_OnlineShopManagement.Network.Update_database;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateProductList
    {
        void UpdateProductList(ProductsInformation pro);
    }
    class ProductsFunction : BaseFunction, IUpdateProductList
    {
        #region Properties
        public string productName { get; set; }
        public string productUnit { get; set; }
        public long productCost { get; set; }
        public long productPrice { get; set; }
        public int IsSelectedIndex { get; set; }
        public int IsSelectedProducerIndex { get; set; }
        public BitmapImage productImage { get; set; }
        public ProductTypeInfomation SelectedProductsType { get; set; }
        public ProducerInformation SelectedProducer { get; set; }
        public ProductsControlViewModel SelectedProduct { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        private ManagingFunctionsViewModel managingFunction;
        private ManagementMenu ManagementMenu;

        public ObservableCollection<ProductsControlViewModel> listActiveItemsProduct { get; set; }
        public ObservableCollection<ProductsControlViewModel> listAllProduct { get; set; }
        public ObservableCollection<ProductTypeInfomation> ItemSourceProductsType { get; set; }
        public ObservableCollection<ProducerInformation> ItemSourceProducer { get; set; }

        #endregion

        #region ICommand
        //Product
        public ICommand OpenAddProductControlCommand { get; set; }
        public ICommand OpenImportProductsCommand { get; set; }
        public ICommand OpenProductsTypeCommand { get; set; }
        //AddProduct
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }
        #endregion
        public ProductsFunction(AppSession session, MongoConnect connect, ManagingFunctionsViewModel managingFunctionsViewModel, ManagementMenu managementMenu) : base(session, connect)
        {
            this._connection= connect;
            this._session= session;        
            managingFunction = managingFunctionsViewModel;
            ManagementMenu = managementMenu;
            IsSelectedIndex = -1;
            IsSelectedProducerIndex = -1;
            ItemSourceProductsType = new ObservableCollection<ProductTypeInfomation>();
            listActiveItemsProduct = new ObservableCollection<ProductsControlViewModel>();
            ItemSourceProducer = new ObservableCollection<ProducerInformation>();
            listAllProduct = new ObservableCollection<ProductsControlViewModel>();
            //Test
            GetData();
            GetAllData();
            GetProductTypeData();
            GetProducerData();
            listActiveItemsProduct.Add(new ProductsControlViewModel(new ProductsInformation("3", "Cocacola", 1, 10000, 5000, "Drink", "Cocacola", "lon"), this));
            //
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);

            OpenAddProductControlCommand = new RelayCommand<Object>(null, OpenAddProductControl);
            OpenProductsTypeCommand = new RelayCommand<Object>(null, OpenProductsType);
            OpenImportProductsCommand = new RelayCommand<Object>(null, OpenImportProducts);
            SelectImageCommand = new RelayCommand<Object>(null,SaveImage);
            SelectedProductsType = null;
            SelectedProducer = null;
        }

        #region Function
        //AddProduct
        public void OpenAddProductControl(Object o = null)
        {
            AddProductControl addProductControl = new AddProductControl();
            addProductControl.DataContext = this;
            DialogHost.Show(addProductControl);
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveProduct);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
        }
        //
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
        public void TextChangedHandle(Object o = null)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(Object o = null)
        {

            if (String.IsNullOrEmpty(productName) ||
                String.IsNullOrEmpty(productUnit)
                || IsSelectedIndex == -1 || IsSelectedProducerIndex == -1
                || String.IsNullOrEmpty(productCost.ToString()) ||
                String.IsNullOrEmpty(productPrice.ToString())
                || productImage == null)
            {
                return false;
            }
            return true;
        }
        public async void SaveProduct(Object o = null)
        {
            if (CheckExist()==false && SelectedProductsType != null && productImage != null && SelectedProducer != null)
            {
                ProductsInformation info = new ProductsInformation("", productName, 0, productPrice, productCost, SelectedProductsType.ID, 
                    SelectedProducer.ID, productUnit, true, await new AutoProductsIDGenerator(_session, _connection.client).Generate());
                RegisterProducts regist = new RegisterProducts(info, _connection.client, _session);
                Task<string> task = regist.register();
                string id = await task;

                Task.WaitAll(task);
                    //Register Image
                    ByteImage bimg = new ByteImage(id, productImage);
                    RegisterByteImage registImage = new RegisterByteImage(bimg, _connection.client, _session);
                    await registImage.register();
                    info.ID = id;
                    listActiveItemsProduct.Add(new ProductsControlViewModel(info, this));
                    listAllProduct.Add(new ProductsControlViewModel(info, this));
                    OnPropertyChanged(nameof(listActiveItemsProduct));
                    Console.WriteLine(id);
                DialogHost.CloseDialogCommand.Execute(null,null);
            }
            else
            {
                SetActive(SelectedProduct);
            }
        }
        public void SaveImage(Object o = null)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "image jpeg(*.jpg)|*.jpg|image png(*.png)|*.png";
            ofd.DefaultExt = ".jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BitmapImage tmp = new BitmapImage(new Uri(ofd.FileName));
                productImage = tmp;
                OnPropertyChanged(nameof(productImage));
                (SaveCommand as RelayCommand<object>).OnCanExecuteChanged();
            }
        }
        public void UpdateProductList(ProductsInformation pro)
        {
            int i = 0;
            if (listActiveItemsProduct.Count > 0)
            {
                foreach (ProductsControlViewModel ls in listActiveItemsProduct)
                {
                    if (ls.product.Equals(pro))
                    {
                        SetUnactive(ls);
                        listAllProduct.Clear();
                        GetAllData();
                        break;
                    }
                    i++;
                }
                listActiveItemsProduct.RemoveAt(i);
                OnPropertyChanged(nameof(listActiveItemsProduct));
            }
            else
            {
                return;
            }
        }
        public async void SetActive(ProductsControlViewModel productinfo)
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("ID", productinfo.ID);
            var update = Builders<ProductsInformation>.Update.Set("isActivated", true);
            UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listActiveItemsProduct.Add(SelectedProduct);
            OnPropertyChanged(nameof(listActiveItemsProduct));
            Console.WriteLine(s);
        }
        public async void SetUnactive(ProductsControlViewModel productinfo)
        {
            if (productinfo.isActivated == true)
            {
                var filter = Builders<ProductsInformation>.Filter.Eq("ID", productinfo.ID);
                var update = Builders<ProductsInformation>.Update.Set("isActivated", false);
                UpdateProductsInformation updater = new UpdateProductsInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                Console.WriteLine(s);
            }
            else Console.WriteLine("Cant execute");
        }
        public bool CheckExist()
        {
            foreach (ProductsControlViewModel ls in listAllProduct)
            {
                if (productName == ls.name && SelectedProductsType.ID == ls.Category && productCost == ls.StockCost && productPrice == ls.price && productUnit == ls.Unit && SelectedProducer.ID == ls.Producer)
                {
                    SelectedProduct = ls;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region DB
        public async void GetData()
        {
            var filter = Builders<ProductsInformation>.Filter.Eq("isActivated", true);
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listActiveItemsProduct.Add(new ProductsControlViewModel(pro, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listActiveItemsProduct));
        }
        public async void GetAllData()
        {
            var filter = Builders<ProductsInformation>.Filter.Empty;
            GetProducts getter = new GetProducts(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductsInformation pro in ls)
            {
                listAllProduct.Add(new ProductsControlViewModel(pro, this));
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(listAllProduct));
        }
        public async void GetProductTypeData()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Empty;
            GetProductType getter = new GetProductType(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProductTypeInfomation pro in ls)
            {
                ItemSourceProductsType.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(ItemSourceProductsType));
        }
        public async void GetProducerData()
        {
            var filter = Builders<ProducerInformation>.Filter.Empty;
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation producer in ls)
            {
                ItemSourceProducer.Add(producer);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(ItemSourceProducer));
        }
        #endregion
    }
}
