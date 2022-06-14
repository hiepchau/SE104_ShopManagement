using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SE104_OnlineShopManagement.Utils
{
    public static class ReverseObservationCollection
    {
       
        public static ObservableCollection<T> Reverse<T>(ObservableCollection<T> oblist)
        {
            List<T> list = new List<T>();
            foreach(T item in oblist) { list.Add(item); }
            list.Reverse();
            oblist.Clear();
            foreach(T item in list) { oblist.Add(item); }
            return oblist;
        }
    }
}
