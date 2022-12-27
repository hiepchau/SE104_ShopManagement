using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104_OnlineShopManagement.Views
{
    /// <summary>
    /// Interaction logic for LanguageComponent.xaml
    /// </summary>
    public partial class LanguageComponent : UserControl
    {
        public LanguageComponent()
        {
            InitializeComponent();
        }

        private void myPopup_LostMouseCapture(object sender, MouseEventArgs e)
        {
            myPopup.IsOpen = false;
        }
    }
}
