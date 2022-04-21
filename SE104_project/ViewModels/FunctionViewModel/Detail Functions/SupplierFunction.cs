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

namespace SE104_OnlineShopManagement.ViewModels.FunctionViewModel.Detail_Functions
{
    class SupplierFunction : BaseFunction
    {
        private MongoConnect _connection;
        private AppSession _session;
        public List<ProducerInformation> listItemsProducer { get; set; }
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
            listItemsProducer = new List<ProducerInformation>();
            //Test
            listItemsProducer.Add(new ProducerInformation("1", "Pepsico", "pepsivn@pepsi.com", "0123456789"));
            //
            OpenAddSupplierControlCommand = new RelayCommand<Object>(null, OpenAddSupplierControl); 
        }
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
        public async void SaveSupplier(object parameter)
        {
            var values = (object[])parameter;
            ProducerInformation info = new ProducerInformation("", values[0].ToString(), values[1].ToString(), values[2].ToString());
            RegisterProducer regist = new RegisterProducer(info, _connection.client, _session);
            string s = await regist.register();
            Console.WriteLine(s);
            //var filter = Builders<ProducerInformation>.Filter.Empty;
            //GetProducer getter = new GetProducer(_connection.client, _session, filter);
            //var ls = getter.Get();
            //foreach (ProducerInformation pro in ls)
            //{
            //    Console.WriteLine(pro.Email);
            //}
            Console.WriteLine("executed");
        }
    }
}
