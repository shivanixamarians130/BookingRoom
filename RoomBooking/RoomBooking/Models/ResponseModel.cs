using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RoomBooking.Models
{
    public class Response<T>
    {
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class ResponseList<T>
    {
        public string ErrorMessage { get; set; }
        public List<T> DataList { get; set; }

        //public void RemoveAt(int v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}