using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    
    public class ImportPOSProductControlViewModel:ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public string quantity { get; set; }
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
            price = product.price;
            DeleteProductsCommand = new RelayCommand<object>(null, deleteproduct);
            quantity = "1";
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
                quantity = "0";
                OnPropertyChanged(nameof(quantity));
            }
            else if (check)
            {
                if(i>= product.quantity)
                {
                    quantity= product.quantity.ToString();
                    OnPropertyChanged(nameof(quantity));
                }
                else if (i < 0)
                {
                    quantity = "0";
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
                if (i <= 0)
                {
                    quantity = "0";
                    OnPropertyChanged(nameof(quantity));

                }
                else
                {
                    quantity = (i - 1).ToString();
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
                    OnPropertyChanged(nameof(quantity));
                }
                
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
        private void deleteproduct(object o)
        {
            _parent.UpdateBoughtList(product);
        }
        #endregion
    }
}
