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

namespace SE104_OnlineShopManagement.Components.TitleBar
{
    /// <summary>
    /// Interaction logic for SearhBar.xaml
    /// </summary>
    public partial class SearhBar : UserControl
    {
        public SearhBar()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SearhBar));

        public string tbWidth
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("tbWidth", typeof(string), typeof(SearhBar));

        public new string ToolTip
        {
            get { return (string)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }

        public static new readonly DependencyProperty ToolTipProperty = DependencyProperty.Register("ToolTip", typeof(string), typeof(SearhBar));
    }
}
