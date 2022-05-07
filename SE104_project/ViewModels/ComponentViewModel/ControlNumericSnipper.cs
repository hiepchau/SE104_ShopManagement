using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class ControlNumericSnipper : ViewModelBase
    {
        public ICommand DecreaseCommand { get; set; }
        public ICommand IncreaseCommand { get; set; }
        public ICommand ChangeCommand { get; set; }
        public string quantity { get; set; }
        protected int maxquantity { get; set; }
        public ControlNumericSnipper(int productquantity)
        {
            quantity = "0";
            maxquantity = productquantity;
            DecreaseCommand = new RelayCommand<object>(null, Decrease);
            IncreaseCommand = new RelayCommand<object>(null, Increase);
            ChangeCommand = new RelayCommand<object>(null, change);
        }
        private void change(object o)
        {
            int i;
            string s = o as string;
            bool check = int.TryParse(s, out i);
            if (!check)
            {
                quantity = "0";
                OnPropertyChanged(nameof(quantity));
            }
            else if (check)
            {
                if (i >= maxquantity)
                {
                    quantity = maxquantity.ToString();
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
                if (i < maxquantity)
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

    }
}
