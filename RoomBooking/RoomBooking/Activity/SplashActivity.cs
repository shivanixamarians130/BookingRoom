using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using RoomBooking.Helpers;
using System;

namespace RoomBooking.Activity
{
    [Activity(Label = "SplashActivity",MainLauncher =false)]
    public class SplashActivity : AppCompatActivity
    {
        public static DateTime dt = new DateTime(); 
         protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
        }

        void OnClick()
        {
            dt = DateTime.Now;
        }
    }
}