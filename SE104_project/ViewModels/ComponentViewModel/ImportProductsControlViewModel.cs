using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    class ImportProductsControlViewModel : ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public ControlNumericSnipper ImportQuantityNumeric { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string StockCost { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public string sum { get; set; }
        public string displayID { get; set; }
        private IUpdateSelectedList _parent;
        #endregion
        #region ICommand
        public ICommand DeleteImportProductsCommand { get; set; }
        #endregion

        public ImportProductsControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.product = product;
            ImportQuantityNumeric = new ControlNumericSnipper(9999);
            ID = product.ID;
            name = product.name;
            quantity = product.quantity;
            StockCost = SeparateThousands(product.StockCost.ToString());
            Unit = product.Unit;
            displayID = product.displayID;
            _parent = parent;
            sum = "";
            GetTypeName();
            DeleteImportProductsCommand = new RelayCommand<Object>(null, deleteImportProducts);
        }


        #region Function
        public void onAmountChanged()
        {
            sum = SeparateThousands((ImportQuantityNumeric.GetDetailNum() * ConvertToNumber(StockCost)).ToString());
            OnPropertyChanged(nameof(sum));
            _parent.isCanExecute();
        }
        public void GetIncreaseQuantityByClick()
        {
            ImportQuantityNumeric.IncreaseCommand.Execute((ImportQuantityNumeric.GetDetailNum().ToString()));
            OnPropertyChanged(nameof(ImportQuantityNumeric));
        }
        public void deleteImportProducts(Object o)
        {
            _parent.UpdateBoughtList(product);
            _parent.isCanExecute();
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
        public async void GetTypeName()
        {
            var filter = Builders<ProductTypeInfomation>.Filter.Eq(x => x.ID, product.Category);
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
        #endregion
    }
}
