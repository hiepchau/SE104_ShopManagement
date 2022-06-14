using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using MongoDB.Driver;
using SE104_OnlineShopManagement.Network.Update_database;
using SE104_OnlineShopManagement.Models;
using System.Threading.Tasks;
using SE104_OnlineShopManagement.Network.Get_database;
using System.Linq;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class InfoStoreFunction : BaseFunction
    {
        #region Properties
        public BitmapImage storeImage { get; set; }
        public string storeName { get; set; }
        public string storeAddress { get; set; }
        public string storePhoneNumber { get; set; }
        public string storeEmail { get; set; }
        public string storeFacebook { get; set; }
        public string storeInstagram { get; set; }
        public string storeTaxNumber { get; set; }
        public string storeWebsite { get; set; }
        public bool isLoaded { get; set; }
        private StoreInformation thisStore { get; set; }
        public List<StoreInformation> listStore { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        #endregion

        #region ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveImageCommand { get; set; }
        #endregion
        public InfoStoreFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            isLoaded = true;
            listStore = new List<StoreInformation>();
            _ = GetStoreData();
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveUser);
            SaveImageCommand = new RelayCommand<object>(null, SaveImage);
        }

        #region Function
        public bool CheckValidSave(object o = null)
        {
            if (String.IsNullOrEmpty(storeName) || String.IsNullOrEmpty(storeAddress) ||
                String.IsNullOrEmpty(storePhoneNumber) || String.IsNullOrEmpty(storeEmail) ||
                String.IsNullOrEmpty(storeTaxNumber) || storeImage == null)
                return false;
            return true;
        }
        public void SaveImage(object o = null)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "image jpeg(*.jpg)|*.jpg|image png(*.png)|*.png";
            ofd.DefaultExt = ".jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BitmapImage tmp = new BitmapImage(new Uri(ofd.FileName));
                storeImage = tmp;
                OnPropertyChanged(nameof(storeImage));
                (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
            }
        }
        public async void SaveUser(object o = null)
        {
            if (thisStore != null)
            {
                var filter = Builders<StoreInformation>.Filter.Eq("ID", thisStore.ID);
                var update = Builders<StoreInformation>.Update
                    .Set("Name", storeName)
                    .Set("Address", storeAddress)
                    .Set("PhoneNumber", storePhoneNumber)
                    .Set("Email", storeEmail)
                    .Set("Facebook", storeFacebook)
                    .Set("Instagram", storeInstagram)
                    .Set("TaxNumber", storeTaxNumber)
                    .Set("Website", storeWebsite);
                UpdateStoreInformation updater = new UpdateStoreInformation(_connection.client, _session, filter, update);
                var s = updater.update();
                //Update Image
                ByteImage bimg = new ByteImage(thisStore.ID, storeImage);
                var filterImage = Builders<ByteImage>.Filter.Eq("ID", thisStore.ID);
                var updateImage = Builders<ByteImage>.Update.Set("data", bimg);
                UpdateImage updaterImage = new UpdateImage(_connection.client, _session, filterImage, updateImage);
                var p = updaterImage.update();
            }
            else
            {
                //Register UserInformation
                StoreInformation info = new StoreInformation("", storeName, storeAddress, storePhoneNumber, storeEmail, storeTaxNumber, storeFacebook="", storeInstagram="", storeWebsite="");
                RegisterStoreInformation regist = new RegisterStoreInformation(info, _connection.client, _session);
                string id = await regist.register();           

                //Register Image
                ByteImage bimg = new ByteImage(id, storeImage);
                RegisterByteImage registImage = new RegisterByteImage(bimg, _connection.client, _session);
                await registImage.register();
                Console.WriteLine(id);
                return;
            }
        }
        private void CheckExist()
        {
            if(listStore.Count > 0) { 
            foreach(StoreInformation store in listStore)
            {       
                thisStore=store;
            }
            storeName = thisStore.name;
            storeAddress = thisStore.address;
            storePhoneNumber = thisStore.phonenumber;
            storeEmail = thisStore.email;
            storeFacebook = thisStore.facebook;
            storeInstagram = thisStore.instagram;
            storeTaxNumber = thisStore.taxnumber;
            storeWebsite = thisStore.website;
            OnPropertyChanged(nameof(storeName));
            OnPropertyChanged(nameof(storeAddress));
            OnPropertyChanged(nameof(storePhoneNumber));
            OnPropertyChanged(nameof(storeEmail));
            OnPropertyChanged(nameof(storeFacebook));
            OnPropertyChanged(nameof(storeInstagram));
            OnPropertyChanged(nameof(storeTaxNumber));
            OnPropertyChanged(nameof(storeWebsite));
            }
        }
        #endregion
        #region DB
        public async Task GetStoreData()
        {
            var filter = Builders<StoreInformation>.Filter.Empty;
            GetStoreInformation getter = new GetStoreInformation(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (StoreInformation store in ls)
            {
                listStore.Add(store);
            }
            CheckExist();
            _ = GetImage();
        }
        public async Task GetImage()
        {
            if (thisStore == null)
            {
                storeImage = new BitmapImage();
                storeImage.BeginInit();
                storeImage.UriSource = new Uri("..//Resources//Images//DefaultNoImage.jpg", UriKind.Relative);
                storeImage.EndInit();
                isLoaded = false;
                OnPropertyChanged(nameof(isLoaded));
                OnPropertyChanged(nameof(storeImage));
            }
            else
            {
                FilterDefinition<ByteImage> filter = Builders<ByteImage>.Filter.Eq(p => p.obID, thisStore.ID);
                GetByteImage getter = new GetByteImage(_connection.client, _session, filter);
                Task<List<ByteImage>> task = getter.Get();
                var ls = await task;
                Task.WaitAll(task);
                if (ls.Count > 0)
                {
                    storeImage = ls.FirstOrDefault().convertByteToImage();
                    OnPropertyChanged(nameof(storeImage));
                    isLoaded = false;
                    OnPropertyChanged(nameof(isLoaded));
                }
                else
                {
                    storeImage = new BitmapImage();
                    storeImage.BeginInit();
                    storeImage.UriSource = new Uri("..//Resources//Images//DefaultNoImage.jpg", UriKind.Relative);
                    storeImage.EndInit();
                    OnPropertyChanged(nameof(storeImage));
                }
            }
        }
        #endregion
    }
}
