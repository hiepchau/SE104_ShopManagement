using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Selling_functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{

    public class POSProductControlViewModel:ViewModelBase
    {
        #region Properties
        public ProductsInformation product { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public int quantity { get; set; }
        private IUpdateSelectedList _parent;
        public bool isLoaded { get; set; }
        public BitmapImage ImageSrc { get; set; }
        #endregion

        #region ICommand
        public ICommand UpdateBoughtCommand { get; set; }
        #endregion

        public POSProductControlViewModel(ProductsInformation product, IUpdateSelectedList parent)
        {
            this.product = product;
            this._parent = parent;
            name = product.name;
            price = product.price;
            quantity = product.quantity;
            UpdateBoughtCommand = new RelayCommand<Object>(null, UpdateBought);
            isLoaded = true;
            getImage(null);
        }

        private async void getImage(object o)
        {
            
            FilterDefinition<ByteImage> filter = Builders<ByteImage>.Filter.Eq(p => p.obID, product.ID);
            GetByteImage getter = new GetByteImage((_parent as BaseFunction).Connect.client, (_parent as BaseFunction).Session, filter);
            Task<List<ByteImage>> task = getter.Get();
            var ls = await task;
            Task.WaitAll(task);
            if (ls.Count > 0)
            {
                ImageSrc = ls.FirstOrDefault().convertByteToImage();
                OnPropertyChanged(nameof(ImageSrc));
                isLoaded = false;
                OnPropertyChanged(nameof(isLoaded));
            }
            else
            {
                ImageSrc = new BitmapImage();
                ImageSrc.BeginInit();
                ImageSrc.UriSource = new Uri("..//Resources//Images//DefaultNoImage.jpg",UriKind.Relative);
                ImageSrc.EndInit();
                OnPropertyChanged(nameof(ImageSrc));
            }
        }

        #region Function
        private void UpdateBought(Object o)
        {               
            _parent.UpdateSelectedList(product);
            _parent.isCanExecute();
        }
        #endregion

    }
}
