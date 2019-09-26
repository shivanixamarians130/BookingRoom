using Android.App;
using Android.Content;
using Android.OS;
using RoomBooking.Activity;
using RoomBooking.Helpers;
using System;

namespace RoomBooking.Services
{
    [Service(Exported = true)]

    public class BackgroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        { 
            var data = DateTime.Now - SplashActivity.dt;
            string DataToBeDisplayed = data.Hours.ToString("00") + " : " +data.Minutes.ToString("00") + " : "+data.Seconds.ToString("00");
          //  System.Threading.Timer timer = new System.Threading.Timer(CheckStatus, null, 0,ConstantHelper.Interval);
            return StartCommandResult.Sticky;
        }

         public void CheckStatus(Object stateInfo)
        {
            BookingDataModel.Instance.SetList();
        }
    }
}