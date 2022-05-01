using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions
{
    public interface IUpdateSelectedList
    {
        void UpdateSelectedList(ProductsInformation pro);
        void UpdateBoughtList(ProductsInformation pro);
    }
    public class SellingViewModel:BaseFunction, IUpdateSelectedList
    {
        #region properties
        public ObservableCollection<POSProductControlViewModel> listProducts { get; set; }
        public ObservableCollection<ImportPOSProductControlViewModel> listbought { get; set; }
        public ProductsInformation selectedProduct { get; set; }
        public string CurrentName { get; set; }
        public string CurrentID { get; set; }
        public string today { get; set; }
        public string clock { get; set; }
        private AppSession _session;
        private MongoConnect _connection;
        #endregion
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
            _session = session;
            _connection = client;
            CurrentName = _session.CurrnetUser.FirstName + " " + _session.CurrnetUser.LastName;
            CurrentID = _session.CurrnetUser.ID;
            today = DateTime.Now.ToString("dd/MM/yyyy");
            listProducts = new ObservableCollection<POSProductControlViewModel>();
            listbought = new ObservableCollection<ImportPOSProductControlViewModel>();
            clockTicking();
            getdata();
            
        }

        private async Task getdata()
        {
            var tmp = new GetProducts(_connection.client, _session, FilterDefinition<ProductsInformation>.Empty);
            var ls = await tmp.Get();
            foreach(ProductsInformation pr in ls)
            {
                listProducts.Add(new POSProductControlViewModel(pr,this));
                
            }
            OnPropertyChanged(nameof(listProducts));
        }

        public void UpdateSelectedList(ProductsInformation pro)
        {
            if (listbought.Count>0)
            {
                foreach(ImportPOSProductControlViewModel pr in listbought)
                {
                    if (pr.product.Equals(pro))
                        return;
                }
            }
            listbought.Add(new ImportPOSProductControlViewModel(pro, this));
            OnPropertyChanged(nameof(listbought));
        }

        public void UpdateBoughtList(ProductsInformation pro)
        {
            int i = 0;
            if (listbought.Count > 0)
            {
                foreach(ImportPOSProductControlViewModel pr in listbought)
                {
                    if (pr.product.Equals(pro))
                    {
                        break;
                    }
                    i++;
                }
                listbought.RemoveAt(i);
                OnPropertyChanged(nameof(listbought));
            }
            else
            {
                return;
            }
        }

        public void clockTicking()
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(changeTime);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void changeTime(Object seneder, EventArgs e)
        {
            clock = DateTime.Now.ToString("HH:mm:ss");
            OnPropertyChanged(nameof(clock));
        }
    }
}
