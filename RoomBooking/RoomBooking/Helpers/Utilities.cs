using Android.Content;
using Android.Widget;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RoomBooking.Helpers
{
    class Utilities
    {
        public static void ShowShortToast(Context context,string message)
        {
          Toast.MakeText(context, message, ToastLength.Short).Show();
        }
    }
}