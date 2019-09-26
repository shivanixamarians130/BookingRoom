using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;
using RoomBooking.Adapters;
using RoomBooking.Helpers;
using RoomBooking.Models;
using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace RoomBooking.Activity
{
    [Activity(Label = "@string/app_name",MainLauncher =false)]
    public class RoomActivity : AppCompatActivity
    {
        ListView ListViewRoom;
        List<RoomModel> roomList;
        ProgressBar ProgressBar;
        RoomAdapter roomAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Crashes.GenerateTestCrash(); 

            if (savedInstanceState != null)
            {
                string val = savedInstanceState.GetString("room_list");
                roomList = string.IsNullOrEmpty(val) ? new List<RoomModel>() : JsonConvert.DeserializeObject<List<RoomModel>>(val);
            }
            SetContentView(Resource.Layout.room_listing_layout);
            ListViewRoom = FindViewById<ListView>(Resource.Id.listViewRoom);
            ProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            ListViewRoom.ItemClick += ListViewRoom_ItemClick;
            LoadRooms();
            AppCenter.Start("ae5fc188-c17a-4407-b1b9-4f5583ed8388",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            string val = JsonConvert.SerializeObject(roomList);
            outState.PutString("room_list",val);
            base.OnSaveInstanceState(outState);
        }
        private void ListViewRoom_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                RoomModel model = roomList[e.Position];
                string SerializedModel = JsonConvert.SerializeObject(model);
                Intent i = new Intent(this, typeof(DisplayBookingActivity));
                i.PutExtra(Resources.GetString(Resource.String.room_data), SerializedModel);
                StartActivity(i);
            }
           catch(Exception exc)
            {

            }
        }

        async private void LoadRooms()
        {
            try
            {
                if(roomList!=null)
                {
                    roomAdapter = new RoomAdapter(this, roomList);
                    ListViewRoom.Adapter = roomAdapter;
                    return;
                }

                ProgressBar.Visibility = Android.Views.ViewStates.Visible;
                Tuple<string,List<RoomModel>> data = await RoomHelper.GetRoomsAsync(this);
                if(!string.IsNullOrEmpty(data.Item1))
                {
                    Toast.MakeText(this, data.Item1, ToastLength.Short).Show();
                    return;
                }
                roomList = data.Item2;
                roomAdapter = new RoomAdapter(this, roomList);
                ListViewRoom.Adapter = roomAdapter;
            }
         catch(Exception exc)
            {

            }
            finally
            {
                ProgressBar.Visibility = Android.Views.ViewStates.Gone;
            }
        }

    }
}