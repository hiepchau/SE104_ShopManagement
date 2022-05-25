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
        public string BillAmount { get; set; }
        public bool isActivated { get; set; }
        public string sumPrice { get; set; }
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
            GetBillAmount();
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
        public async void GetBillAmount()
        {
            int sum = 0;
            long sumPrice = 0;
            //Get Product
            await GetProduct();
            //Get StockDetails
            await GetStockDetail();

            foreach (ProductsInformation pro in listproducts) 
            {
                foreach (StockDetails stock in liststock)
                {
                    if (pro.ID.Equals(stock.productID))
                    {
                        sumPrice += stock.sumPrice;
                        sum++;
                    }
                }
            }          
            this.sumPrice = SeparateThousands(sumPrice.ToString());
            BillAmount = SeparateThousands(sum.ToString());
            OnPropertyChanged(nameof(sumPrice));
            OnPropertyChanged(nameof(BillAmount));
        }
        public async Task GetProduct()
        {
            var productfilter = Builders<ProductsInformation>.Filter.Eq("ProductProvider", ID);
            GetProducts productgetter = new GetProducts((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, productfilter);
            var ls = await productgetter.Get();
            foreach(var product in ls)
            {
                listproducts.Add(product);
            }
        }
        public async Task GetStockDetail()
        {
            var filter = Builders<StockDetails>.Filter.Empty;
            GetStockingDetail getter = new GetStockingDetail((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            foreach(StockDetails stock in ls)
            {
                liststock.Add(stock);
            }
        }
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        #endregion
    }
}
