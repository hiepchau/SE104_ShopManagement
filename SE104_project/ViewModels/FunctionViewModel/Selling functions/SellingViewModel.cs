using MongoDB.Driver;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Get_database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions
{
    public class SellingViewModel:BaseFunction
    {
        #region properties
        public ObservableCollection<ProductsInformation> listProducts { get; set; }
        public ProductsInformation selectedProduct { get; set; }
        private AppSession _session;
        private MongoConnect _connection;
        #endregion
        public SellingViewModel(AppSession session, MongoConnect client) : base(session, client)
        {
            _session = session;
            _connection = client;
            listProducts = new ObservableCollection<ProductsInformation>();
            listProducts.Clear();
            getdata();
            
        }

        private async Task getdata()
        {
            var tmp = new GetProducts(_connection.client, _session, FilterDefinition<ProductsInformation>.Empty);
            var ls = await tmp.Get();
            foreach(ProductsInformation pr in ls)
            {
                listProducts.Add(pr);
                OnPropertyChanged(nameof(listProducts));
            }
        }

    }
}
