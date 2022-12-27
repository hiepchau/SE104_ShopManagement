using SE104_OnlineShopManagement.Components.Controls;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SE104_OnlineShopManagement.Views.Pages.POSComponents
{
    /// <summary>
    /// Interaction logic for POSPage.xaml
    /// </summary>
    public partial class POSPage : UserControl
    {
        public POSPage()
        {
            InitializeComponent();
        }

        public void HandleValueChanged(object sender, RoutedEventArgs e)
        {
            var data = e.OriginalSource;
            if (data != null)
            {
                Console.WriteLine("Event detected");
                int i;
                if (int.TryParse((data as NumericSnipperControl).currentvalue.ToString(), out i))
                {
                    Console.WriteLine((data as NumericSnipperControl).currentvalue.ToString());
                    (this.DataContext as SellingViewModel).getTotalPay();
                }
            }
        }
    }
}
