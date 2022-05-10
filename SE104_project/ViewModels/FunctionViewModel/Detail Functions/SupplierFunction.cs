using MaterialDesignThemes.Wpf;
using SE104_OnlineShopManagement.Commands;
using SE104_OnlineShopManagement.Components;
using SE104_OnlineShopManagement.Models.ModelEntity;
using SE104_OnlineShopManagement.Network;
using SE104_OnlineShopManagement.Network.Insert_database;
using SE104_OnlineShopManagement.Network.Get_database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SE104_OnlineShopManagement.Services;
using SE104_OnlineShopManagement.ViewModels.ComponentViewModel;
using SE104_OnlineShopManagement.Network.Update_database;

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    public interface IUpdateSuplierList
    {
        void UpdateSuplierList(ProducerInformation producer);
    }
    class SupplierFunction : BaseFunction, IUpdateSuplierList
    {
        #region Properties
        public string supplierName { get; set; }
        public string supplierAddress { get; set; }
        public string supplierPhone { get; set; }
        public string supplierMail { get; set; }
        public int IsSelectedIndex { get; set; }
        public SupplierControlViewModel selectedProducer { get; set; }
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<SupplierControlViewModel> listActiveItemsProducer { get; set; }
        public ObservableCollection<SupplierControlViewModel> listAllProducer { get; set; }
        #endregion
        #region ICommand
        //Supplier
        public ICommand OpenAddSupplierControlCommand { get; set; }
        //AddSupplier
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand TextChangedCommand { get; set; }

        #endregion
        public SupplierFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            IsSelectedIndex = -1;
            listAllProducer = new ObservableCollection<SupplierControlViewModel>();
            listActiveItemsProducer = new ObservableCollection<SupplierControlViewModel>();
            //Test
            GetData();
            GetAllData();
            listActiveItemsProducer.Add(new SupplierControlViewModel(new ProducerInformation("1", "Pepsico", "pepsivn@pepsi.com", "0123456789",""), this));
            //
            TextChangedCommand = new RelayCommand<Object>(null, TextChangedHandle);
            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl); 
        }
        #region Function
        public void OpenAddSupplierControl(Object o = null)
        {
            AddSupplierControl addSupplierControl = new AddSupplierControl();
            addSupplierControl.DataContext = this;
            DialogHost.Show(addSupplierControl);
            SaveCommand = new RelayCommand<Object>(CheckValidSave, SaveSupplier);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                SetNull();
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public void TextChangedHandle(Object o = null)
        {
            (SaveCommand as RelayCommand<Object>).OnCanExecuteChanged();
        }
        public bool CheckValidSave(Object o = null)
        {
            if (String.IsNullOrEmpty(supplierName) || String.IsNullOrEmpty(supplierAddress)
                || String.IsNullOrEmpty(supplierMail) || String.IsNullOrEmpty(supplierPhone))
            {
                return false;
            }
            return true;
        }
        public async void SaveSupplier(object o = null)
        {
            if (CheckExist() == false)
            {
                ProducerInformation info = new ProducerInformation("", supplierName, supplierMail, supplierPhone, supplierAddress, true, await new AutoProducerIDGenerator(_session, _connection.client).Generate());
                RegisterProducer regist = new RegisterProducer(info, _connection.client, _session);
                string s = await regist.register();
                listActiveItemsProducer.Add(new SupplierControlViewModel(info, this));
                listAllProducer.Add(new SupplierControlViewModel(info, this));
                OnPropertyChanged(nameof(listActiveItemsProducer));
                Console.WriteLine(s);
            }
            else
            {
                SetActive(selectedProducer);
            }
        }
        public void UpdateSuplierList(ProducerInformation producer)
        {
            int i = 0;
            if (listActiveItemsProducer.Count > 0)
            {
                foreach (SupplierControlViewModel ls in listActiveItemsProducer)
                {
                    if (ls.producer.Equals(producer))
                    {
                        SetUnactive(ls);
                        listAllProducer.Clear();
                        GetAllData();
                        break;
                    }
                    i++;
                }
                listActiveItemsProducer.RemoveAt(i);
                OnPropertyChanged(nameof(listActiveItemsProducer));
            }
            else
            {
                return;
            }
        }
        public async void SetActive(SupplierControlViewModel producerinfo)
        {
            var filter = Builders<ProducerInformation>.Filter.Eq("ID", producerinfo.ID);
            var update = Builders<ProducerInformation>.Update.Set("isActivated", true);
            UpdateProducerInformation updater = new UpdateProducerInformation(_connection.client, _session, filter, update);
            var s = await updater.update();
            listActiveItemsProducer.Add(selectedProducer);
            OnPropertyChanged(nameof(listActiveItemsProducer));
            Console.WriteLine(s);
        }
        public async void SetUnactive(SupplierControlViewModel producerinfo)
        {
            if (producerinfo.isActivated == true)
            {
                var filter = Builders<ProducerInformation>.Filter.Eq("ID", producerinfo.ID);
                var update = Builders<ProducerInformation>.Update.Set("isActivated", false);
                UpdateProducerInformation updater = new UpdateProducerInformation(_connection.client, _session, filter, update);
                var s = await updater.update();
                Console.WriteLine(s);
            }
            else Console.WriteLine("Cant execute");
        }
        public bool CheckExist()
        {
            foreach (SupplierControlViewModel ls in listAllProducer)
            {
                if (supplierName == ls.Name && supplierAddress == ls.Address && supplierMail == ls.Email && supplierPhone == ls.PhoneNumber)
                {
                    selectedProducer = ls;
                    return true;
                }
            }
            return false;
        }

        public void SetNull()
        {
            supplierName = "";
            supplierAddress = "";
            supplierMail = "";
            supplierPhone = "";
            OnPropertyChanged(nameof(supplierName));
            OnPropertyChanged(nameof(supplierAddress));
            OnPropertyChanged(nameof(supplierMail));
            OnPropertyChanged(nameof(supplierPhone));
        }
        #endregion
        #region DB
        public async void GetData()
        {
            var filter = Builders<ProducerInformation>.Filter.Eq("isActivated",true);
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation pro in ls)
            {
                listActiveItemsProducer.Add(new SupplierControlViewModel(pro,this));
            }
            OnPropertyChanged(nameof(listActiveItemsProducer));
        }
        public async void GetAllData()
        {
            var filter = Builders<ProducerInformation>.Filter.Empty;
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation pro in ls)
            {
                listAllProducer.Add(new SupplierControlViewModel(pro, this));
            }
        }
        #endregion
    }
}
