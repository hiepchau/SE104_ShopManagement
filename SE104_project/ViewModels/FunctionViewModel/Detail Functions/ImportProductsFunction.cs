using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{

    class ImportProductsFunction : BaseFunction, IUpdateSelectedList
    {
        #region Properties
        private MongoConnect _connection;
        private AppSession _session;
        public long totalReceipt { get; set; }
        public ObservableCollection<ProducerInformation> ItemSourceSupplier { get; set; }
        public ObservableCollection<POSProductControlViewModel> listProducts { get; set; }
        public ObservableCollection<ImportProductsControlViewModel> listItemsImportProduct { get; set; }
        public string searchString { get; set; }
        #endregion

        #region ICommand
        public ICommand OpenAddReceiptControlCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        //AddReceiptControl
        public ICommand ExitCommand { get; set; }
        public ICommand OpenAddSupplierControlCommand { get; set; }
        #endregion
        public ImportProductsFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            _connection = connect;
            _session = session;
            ItemSourceSupplier = new ObservableCollection<ProducerInformation>();
            listItemsImportProduct = new ObservableCollection<ImportProductsControlViewModel>();
            listProducts = new ObservableCollection<POSProductControlViewModel>();
 
            GetProducerInfo();
            getdata();
            SearchCommand = new RelayCommand<Object>(null, search);
            
            OpenAddReceiptControlCommand = new RelayCommand<Object>(isEmptyImportList, OpenAddReceiptControl);
        }

        #region Function
        public void OpenAddReceiptControl(Object o = null)
        {
            AddReceiptControl addReceiptControl = new AddReceiptControl();
            addReceiptControl.DataContext = this;
            DialogHost.Show(addReceiptControl);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            totalReceipt = 0;
            foreach(ImportProductsControlViewModel pr in listItemsImportProduct)
            {
                totalReceipt += pr.sum;
            }
            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl);
        }
        public void OpenAddSupplierControl(Object o = null)
        {
            AddSupplierControl addSupplierControl = new AddSupplierControl();
            addSupplierControl.DataContext = this;
            DialogHost.Show(addSupplierControl);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public void isCanExecute()
        {
            (OpenAddReceiptControlCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool isEmptyImportList(Object o)
        {
            if (listItemsImportProduct.Count > 0)
                return true;
            OnPropertyChanged(nameof(listItemsImportProduct));
            return false;
        }
        public void UpdateSelectedList(ProductsInformation pro)
        {
            if (listItemsImportProduct.Count > 0)
            {
                foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
                {
                    if (pr.product.Equals(pro))
                        return;
                }
            }
            listItemsImportProduct.Add(new ImportProductsControlViewModel(pro, this));
            OnPropertyChanged(nameof(listItemsImportProduct));
        }
        public void UpdateBoughtList(ProductsInformation pro)
        {
            int i = 0;
            if (listItemsImportProduct.Count > 0)
            {
                foreach (ImportProductsControlViewModel pr in listItemsImportProduct)
                {
                    if (pr.product.Equals(pro))
                    {
                        break;
                    }
                    i++;
                }
                listItemsImportProduct.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsImportProduct));
            }
            else
            {
                return;
            }
        }
        private async void search(object o)
        {
            searchString = o.ToString();
            if (string.IsNullOrEmpty(searchString))
            {
                await getdata();
            }
            else
            {
                await getsearchdata();
            }
        }
        #endregion

        #region DB
        public async void GetProducerInfo()
        {
            var filter = Builders<ProducerInformation>.Filter.Empty;
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation pro in ls)
            {
                ItemSourceSupplier.Add(pro);
            }
            Console.Write("Executed");
            OnPropertyChanged(nameof(ItemSourceSupplier));
          
        }
        private async Task getdata()
        {
            var tmp = new GetProducts(_connection.client, _session, FilterDefinition<ProductsInformation>.Empty);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr, this));

            }
            OnPropertyChanged(nameof(listProducts));
        }
        private async Task getsearchdata()
        {
            listProducts.Clear();
            OnPropertyChanged(nameof(listProducts));
            FilterDefinition<ProductsInformation> filter = Builders<ProductsInformation>.Filter.Eq(x => x.name, searchString);
            var tmp = new GetProducts(_connection.client, _session, filter);
            var ls = await tmp.Get();
            foreach (ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr, this));

            }
            OnPropertyChanged(nameof(listProducts));
        }

        #endregion
    }
}
