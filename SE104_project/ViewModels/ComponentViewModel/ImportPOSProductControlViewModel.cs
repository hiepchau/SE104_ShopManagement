using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    
    public class ImportPOSProductControlViewModel:ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string unit { get; set; }
        public string sum { get; set; }
        public string Category { get; set; }
        public BitmapImage ImageSrc { get; set; }
        public bool isLoaded { get; set; }

        #endregion

        #region ICommand
        public ICommand DeleteProductsCommand { get; set; }
        public ICommand DecreaseCommand { get; set; }
        public ICommand IncreaseCommand { get; set; }
        public ICommand ChangeCommand { get; set; }
        #endregion

        private IUpdateSelectedList _parent;
        public ImportPOSProductControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.ID = product.ID;
            this.product = product;
            this._parent = parent;
            name = product.name;
            price = SeparateThousands(product.price.ToString());
            DeleteProductsCommand = new RelayCommand<object>(null, deleteproduct);
            quantity = "1";
            sum = price;
            unit = product.Unit;
            GetTypeName();
            DecreaseCommand = new RelayCommand<object>(null, Decrease);
            IncreaseCommand = new RelayCommand<object>(null, Increase);
            ChangeCommand = new RelayCommand<object>(null, change);
        }
        #region Function
        
        private void change(object o)
        {
            int i;
            string s = o as string;
            bool check = int .TryParse(s, out i);
            if (!check)
            {
                quantity = "1";
                OnPropertyChanged(nameof(quantity));
            }
            else if (check)
            {

                if(i > product.quantity)
                {
                    quantity= product.quantity.ToString();
                    OnPropertyChanged(nameof(quantity));
                }
                else if (i < 1)
                {
                    quantity = "1";
                    OnPropertyChanged(nameof(quantity));
                }
            }
        }
        private void Decrease(Object o)
        {
            string s = o.ToString();
            int i = 0;
            if (int.TryParse(s, out i))
            {
                if (i <= 1)
                {
                    quantity = "1";
                    _parent.updateTotal();
                    OnPropertyChanged(nameof(quantity));

                }
                else
                {
                    quantity = (i - 1).ToString();
                    _parent.updateTotal();
                    OnPropertyChanged(nameof(quantity));
                }
            }
            
        }

        private void Increase(Object o)
        {
            string s = o.ToString();
            int i = 0;
            if (int.TryParse(s, out i))
            {
                if (i < product.quantity)
                {
                    quantity = (i + 1).ToString();
                    _parent.updateTotal();
                    OnPropertyChanged(nameof(quantity));
                }
            }
        }
        public async void GetTypeName()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq(x => x.ID, product.ID);
            GetProductType getter = new GetProductType((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            var ls = await getter.Get();
            if (ls != null && ls.Count > 0)
            {
                Category = ls.First().name;
                OnPropertyChanged(nameof(Category));
            }
            else
            {
                return;
            }
        }
           
        public void GetIncreaseQuantityByClick()
        {
            if (GetDetailNum() < product.quantity)
            {
                quantity = (GetDetailNum() + 1).ToString();
                OnPropertyChanged(nameof(quantity));
            }
        }
        public int GetDetailNum()
        {
            int id;
            if (int.TryParse((string)quantity, out id))
            {
                return id;
            }
            else return 0;
        }
        public void onAmountChanged()
        {
            sum = SeparateThousands((GetDetailNum() * ConvertToNumber(price)).ToString());
            OnPropertyChanged(nameof(sum));
            
        }

        private void deleteproduct(object o)
        {
            _parent.UpdateBoughtList(product);
        }
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(ConvertToNumber(text).ToString(), System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        public long ConvertToNumber(string str)
        {
            string[] s = str.Split(',');
            string tmp = "";
            foreach (string a in s)
            {
                tmp += a;
            }

            return long.Parse(tmp);
        }
        #endregion
    }
}
