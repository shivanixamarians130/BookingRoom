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
   public class RoomModel
    {
        public int Id;
        public string Name;

        public RoomModel(int id,string name)
        {
            Id = id;
            Name = name;
        }
    }
}