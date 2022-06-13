using MongoDB.Driver;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Models;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network.Get_database;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel;
using SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SE104_OnlineShopManagement.ViewModels.ComponentViewModel
{
    public class TopSaleProductControlViewModel : ViewModelBase
    {
        private BaseFunction _parent;
        #region Properties
        public ProductsInformation product { get; set; }
        public string ID { get; private set; }
        public string name { get; set; }
        public string displayID { get; set; }
        public int amount { get; set; }
        public bool isLoaded { get; set; }
        public BitmapImage ImageSrc { get; set; }
        #endregion

        public TopSaleProductControlViewModel(ProductsInformation products, BaseFunction parent)
        {
            this.product = products;
            ID = product.ID;
            name = product.name;
            amount = 0;
            displayID = product.displayID;
            _parent = parent;
            getImage();
        }

        #region Function
        public string SeparateThousands(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                ulong valueBefore = ulong.Parse(text, System.Globalization.NumberStyles.AllowThousands);
                string res = String.Format(culture, "{0:N0}", valueBefore);
                return res;
            }
            return "";
        }
        private async void getImage()
        {
            FilterDefinition<ByteImage> filter = Builders<ByteImage>.Filter.Eq(p => p.obID, product.ID);
            GetByteImage getter = new GetByteImage(_parent.Connect.client, _parent.Session, filter);
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
                ImageSrc.UriSource = new Uri("..//Resources//Images//DefaultNoImage.jpg", UriKind.Relative);
                ImageSrc.EndInit();
                OnPropertyChanged(nameof(ImageSrc));
            }
        }
        #endregion
    }
}
