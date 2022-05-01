using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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
        private AppSession _session;
        private MongoConnect _connection;
        #endregion
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
            _session = session;
            _connection = client;
            listProducts = new ObservableCollection<POSProductControlViewModel>();
            listbought = new ObservableCollection<ImportPOSProductControlViewModel>();
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
    }
}
