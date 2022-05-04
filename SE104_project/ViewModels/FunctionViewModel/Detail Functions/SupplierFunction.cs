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
        private MongoConnect _connection;
        private AppSession _session;
        public ObservableCollection<SupplierControlViewModel> listItemsProducer { get; set; }
        #endregion
        #region ICommand
        //Supplier
        public ICommand OpenAddSupplierControlCommand { get; set; }
        //AddSupplier
        public ICommand SaveCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        #endregion
        public SupplierFunction(AppSession session, MongoConnect connect) : base(session, connect)
        {
            this._connection = connect;
            this._session = session;
            listItemsProducer = new ObservableCollection<SupplierControlViewModel>();
            //Test
            GetData();
            listItemsProducer.Add(new SupplierControlViewModel(new ProducerInformation("1", "Pepsico", "pepsivn@pepsi.com", "0123456789",""), this));
            //
            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl); 
        }
        #region Function
        public void OpenAddSupplierControl(Object o = null)
        {
            AddSupplierControl addSupplierControl = new AddSupplierControl();
            addSupplierControl.DataContext = this;
            DialogHost.Show(addSupplierControl);
            SaveCommand = new RelayCommand<Object>(null,SaveSupplier);
            ExitCommand = new RelayCommand<Object>(null, exit =>
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
            });

        }
        public async void SaveSupplier(object o = null)
        {
            ProducerInformation info = new ProducerInformation(await new AutoProducerIDGenerator(_session,_connection.client).Generate()
                , supplierName,supplierMail,supplierPhone,supplierAddress);
            RegisterProducer regist = new RegisterProducer(info, _connection.client, _session);
            string s = await regist.register();
            listItemsProducer.Add(new SupplierControlViewModel(info, this));
            OnPropertyChanged(nameof(listItemsProducer));
            Console.WriteLine(s);
        }
        public void UpdateSuplierList(ProducerInformation producer)
        {
            int i = 0;
            if (listItemsProducer.Count > 0)
            {
                foreach (SupplierControlViewModel ls in listItemsProducer)
                {
                    if (ls.producer.Equals(producer))
                    {
                        break;
                    }
                    i++;
                }
                listItemsProducer.RemoveAt(i);
                OnPropertyChanged(nameof(listItemsProducer));
            }
            else
            {
                return;
            }
        }

        #endregion
        #region DB
        public async void GetData()
        {
            var filter = Builders<ProducerInformation>.Filter.Empty;
            GetProducer getter = new GetProducer(_connection.client, _session, filter);
            var ls = await getter.Get();
            foreach (ProducerInformation pro in ls)
            {
                listItemsProducer.Add(new SupplierControlViewModel(pro,this));
            }
            OnPropertyChanged(nameof(listItemsProducer));
        }
        #endregion
    }
}
