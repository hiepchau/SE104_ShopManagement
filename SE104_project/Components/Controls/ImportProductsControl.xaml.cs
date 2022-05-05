using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104_OnlineShopManagement.Components.Controls
{
    /// <summary>
    /// Interaction logic for ImportProductsControl.xaml
    /// </summary>
    public partial class ImportProductsControl : UserControl
    {
        public ImportProductsControl()
        {
            InitializeComponent();
        }

        public void HandleValueChanged(object sender, RoutedEventArgs e)
        {
            var data = e.OriginalSource;
            if(data != null)
            {
                Console.WriteLine("Event detected");
                int i;
                if (int.TryParse((data as NumericSnipperControl).currentvalue.ToString(), out i))
                {
                    Console.WriteLine((data as NumericSnipperControl).currentvalue.ToString());
                    (this.DataContext as ImportProductsControlViewModel).onAmountChanged();
                }
            }
        }
    }
}
