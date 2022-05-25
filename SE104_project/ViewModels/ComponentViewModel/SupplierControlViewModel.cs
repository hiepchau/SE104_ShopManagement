using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class SupplierControlViewModel : ViewModelBase
    {
        #region Properties
        public ProducerInformation producer { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string displayID { get; set; }
        public string Address { get; set; }
        public int BillAmount { get; set; }
        public bool isActivated { get; set; }
        private IUpdateSuplierList _parent;
        private List<ProductsInformation> listproducts { get; set; }
        private List<StockDetails> liststock { get; set; }
        #endregion

        #region ICommand
        public ICommand EditSupplierCommand { get; set; }
        public ICommand DeleteSupplierCommand { get; set; }
        #endregion

        public SupplierControlViewModel(ProducerInformation producer, IUpdateSuplierList parent)
        {
            this.producer = producer;
            ID = producer.ID;
            Name = producer.Name;
            PhoneNumber = producer.PhoneNumber;
            displayID = producer.displayID;
            isActivated = producer.isActivated;
            Address = producer.Address;
            Email = producer.Email;
            _parent = parent;
            listproducts=new List<ProductsInformation>();
            liststock=new List<StockDetails>();
            _ = GetBillAmount();
            EditSupplierCommand = new RelayCommand<Object>(null, EditSupplier);
            DeleteSupplierCommand = new RelayCommand<Object>(null, DeleteSupplier);
        }

        #region Function
        public void DeleteSupplier(Object o)
        {
            _parent.UpdateSuplierList(producer);
        }
        public void EditSupplier(Object o)
        {
            _parent.EditSupplier(producer);
        }
        public async Task GetBillAmount()
        {
            int sum = 0;
            //Get Product
            await GetProduct();
            //Get StockDetails
            await GetStockDetail();

            foreach (ProductsInformation pro in listproducts) 
            {
                Console.WriteLine(pro.ID);
                foreach (StockDetails stock in liststock)
                {
                    Console.WriteLine(stock.productID);
                    if (pro.ID.Equals(stock.productID))
                    {
                        sum++;
                    }
                }
            }          
            BillAmount = sum;
            Console.Write(BillAmount);
            OnPropertyChanged(nameof(BillAmount));
        }
        public async Task GetProduct()
        {
            var productfilter = Builders<ProductsInformation>.Filter.Eq("ProductProvider", ID);
            GetProducts productgetter = new GetProducts((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, productfilter);
            var producttask = productgetter.Get();
            var ls1 = await producttask;
            Task.WaitAll(producttask);
            foreach(var product in ls1)
            {
                listproducts.Add(product);
            }
        }
        public async Task GetStockDetail()
        {
            var filter = Builders<StockDetails>.Filter.Empty;
            GetStockingDetail getter = new GetStockingDetail((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            foreach(StockDetails stock in ls)
            {
                liststock.Add(stock);
            }
        }
        #endregion
    }
}
