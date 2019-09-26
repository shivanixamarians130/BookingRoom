using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RoomBooking.Adapters;
using RoomBooking.Helpers;
using RoomBooking.Models;
using RoomBooking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace RoomBooking.Activity
{
    [Activity(MainLauncher = true, Theme = "@style/AppTheme.NoActionBar")]
    public class DisplayBookingActivity : AppCompatActivity
    {
        public TextView TextViewUpcomingBookings;
        private TextView TextViewCurrentTime;
        private TextView TextViewCurrentDay;
        private TextView TextViewCurrentDate;
        private ListView ListViewBookings;
        public LinearLayout OngoingLinearLayout;
        private LinearLayout AvailableLinearLayout;
        private TextView TextViewOngoingTimeInterval;
        private TextView TextViewOngoingCountDown;
        private TextView TextViewBookedBy;
        private TextView TextViewAvailableCountDown;
        private TextView TextViewNoDataFound;
        private View ViewMeetingSideBar;
        private List<BookingDetailModel> BookingList = new List<BookingDetailModel>();
        private BookingDetailModel CurrentBooking;
        private RoomModel CurrentRoom;
        public ProgressBar ProgressBar;
        private Timer _timer;
        private bool IsCurrentBookingOngoing = false;
        private DisplayBookingAdapter adapter;
        public LinearLayout WholeParent;
        private TextView TextViewPurpose;
        private TextView RoomNameTextView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.TurnScreenOn);
               // CurrentRoom = JsonConvert.DeserializeObject<RoomModel>(Intent.GetStringExtra(Resources.GetString(Resource.String.room_data)));
                 CurrentRoom = new RoomModel(2, "Meeting Room");
                SetContentView(Resource.Layout.display_booking_layout);
                ListViewBookings = FindViewById<ListView>(Resource.Id.listViewMeetings);
                OngoingLinearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutOngoingMeeting);
                AvailableLinearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutAvailableMeeting);
                TextViewOngoingTimeInterval = FindViewById<TextView>(Resource.Id.textViewOngoingTimeInterval);
                TextViewOngoingCountDown = FindViewById<TextView>(Resource.Id.textViewOngoingCountDown);
                RoomNameTextView = FindViewById<TextView>(Resource.Id.textViewRoomName);
                TextViewPurpose = FindViewById<TextView>(Resource.Id.textViewPurpose);
                TextViewBookedBy = FindViewById<TextView>(Resource.Id.textViewBookedBy);
                TextViewCurrentTime = FindViewById<TextView>(Resource.Id.textViewCurrentTime);
                TextViewCurrentDay = FindViewById<TextView>(Resource.Id.textViewCurrentDay);
                TextViewCurrentDate = FindViewById<TextView>(Resource.Id.textViewCurrentDate);
                TextViewAvailableCountDown = FindViewById<TextView>(Resource.Id.textViewShowAvailavleCountDown);
                TextViewNoDataFound = FindViewById<TextView>(Resource.Id.textViewNoDataFound);
                TextViewUpcomingBookings = FindViewById<TextView>(Resource.Id.textViewUpcomingMeetings);
                ViewMeetingSideBar = FindViewById<View>(Resource.Id.viewMeetingState);
                ProgressBar = ProgressBar ?? FindViewById<ProgressBar>(Resource.Id.progressBar);
                TextViewCurrentDay.Text = DateTime.Now.DayOfWeek.ToString();
                TextViewCurrentDate.Text = DateTime.Now.ToString(Resources.GetString(Resource.String.date_format));
                WholeParent = FindViewById<LinearLayout>(Resource.Id.linearLayoutCompleteParent);
                RoomNameTextView.Text = CurrentRoom.Name;
                adapter = new DisplayBookingAdapter(this, BookingList, ListViewBookings.Height);
                WholeParent.Visibility = ViewStates.Gone;
                ListViewBookings.Adapter = adapter;
                BookingDataModel.Init(CurrentRoom, this);
                if (savedInstanceState != null)
                {
                    string val = savedInstanceState.GetString(Resources.GetString(Resource.String.booking_list));
                    BookingList.AddRange(string.IsNullOrEmpty(val) ? new List<BookingDetailModel>() : JsonConvert.DeserializeObject<List<BookingDetailModel>>(val));
                    ProgressBar.Visibility = ViewStates.Gone;
                    AssignBookings();
                }
                System.Threading.Timer timer = new System.Threading.Timer(CheckStatus, null, 0, ConstantHelper.Interval);
            //    var intent = new Intent(this, typeof(BackgroundService));
              //  StartService(intent);
            }
            catch (Exception exc)
            {

                Crashes.TrackError(exc);
                
            }
        }
        public void CheckStatus(Object stateInfo)
        {
            try
            {
                BookingDataModel.Instance.SetList();
            }
            catch (Exception exc)
            {
                Crashes.TrackError(exc);
            }
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            try
            {
                if (IsCurrentBookingOngoing && CurrentBooking != null)
                {
                    BookingList.Add(CurrentBooking);
                    BookingList = BookingList.OrderBy(x => x.start).ToList();
                }
                string val = JsonConvert.SerializeObject(BookingList);
                outState.PutString(Resources.GetString(Resource.String.booking_list), val);
                base.OnSaveInstanceState(outState);
            }
          catch(Exception exc)
            {
                Crashes.TrackError(exc);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            BookingDataModel.InvokeSetList += BookingDataModel_InvokeSetList;
        }
       
        private void BookingDataModel_InvokeSetList(object sender, EventArgs e)
        {
            try
            {
                List<BookingDetailModel> list = sender as List<BookingDetailModel>;
                BookingList.Clear();
                if (list != null && list.Count > 0)
                {
                    BookingList.AddRange(list.OrderBy(x => x.start).ToList());
                    AssignBookings();
                    ShowBookingsIfPresent(true);
                }
                else
                {
                    ShowBookingsIfPresent(false);
                }
            }
            catch (Exception exc)
            {
                Crashes.TrackError(exc);
            }
        }
        private void ShowBookingsIfPresent(bool isBookingPresent)
        {
            if (isBookingPresent)
            {
                this.RunOnUiThread(() =>
                {
                    WholeParent.Visibility = ViewStates.Visible;
                    TextViewNoDataFound.Visibility = ViewStates.Gone;
                });
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    WholeParent.Visibility = ViewStates.Gone;
                    TextViewNoDataFound.Visibility = ViewStates.Visible;
                });
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
            BookingDataModel.InvokeSetList -= BookingDataModel_InvokeSetList;
        }
        void AssignBookings()
        {
            try
            {
                if (BookingList != null && BookingList.Count > 0)
                {
                    ShowBookingsIfPresent(true);
                    BookingDetailModel model = BookingList.Where(s => DateTime.Now >= s.start && DateTime.Now < s.end).ToList().FirstOrDefault();
                    if (model != null)
                    {
                        CurrentBooking = model;
                        BookingList.Remove(CurrentBooking);
                        this.RunOnUiThread(() =>
                        {
                              adapter.NotifyDataSetChanged();
                            OngoingLinearLayout.Visibility = ViewStates.Visible;
                            AvailableLinearLayout.Visibility = ViewStates.Gone;
                            ViewMeetingSideBar.SetBackgroundColor(color: Resources.GetColor(Resource.Color.colorOrange));
                            TextViewPurpose.Text = model.purpose;
                            TextViewBookedBy.Text = Resources.GetString(Resource.String.booked_via) + " " + model.reserved_by;
                            TextViewOngoingTimeInterval.Text = model.start.ToString(Resources.GetString(Resource.String.time_interval_format)) + " - " + model.end.ToString(Resources.GetString(Resource.String.time_interval_format));
                        });
                        IsCurrentBookingOngoing = true;
                        StartTimer();
                    }
                    else
                    {
                        this.RunOnUiThread(() =>
                        {
                            adapter.NotifyDataSetChanged();
                            OngoingLinearLayout.Visibility = ViewStates.Gone;
                            AvailableLinearLayout.Visibility = ViewStates.Visible;
                            ViewMeetingSideBar.SetBackgroundColor(color: Resources.GetColor(Resource.Color.colorGreen));
                        });
                        IsCurrentBookingOngoing = false;
                        CurrentBooking = BookingList[0];
                        StartTimer();
                    }
                    CheckUpcomingBookings();
                }
                else
                {
                    ShowBookingsIfPresent(false);
                }
            }
            catch (Exception exc)
            {
                Crashes.TrackError(exc);
            }
        }

        void CheckUpcomingBookings()
        {
            if (BookingList == null || BookingList.Count == 0)
            {
                TextViewUpcomingBookings.Visibility = ViewStates.Gone;
            }
            else
            {
                TextViewUpcomingBookings.Visibility = ViewStates.Visible;
            }
        }

        void StartTimer()
        {
            _timer = new Timer();
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                DateTime dt = new DateTime();
                if (IsCurrentBookingOngoing)
                    dt = CurrentBooking.end;
                else
                    dt = CurrentBooking.start;

                TimeSpan ts = dt - DateTime.Now;
                string hour = ts.Hours.ToString(Resources.GetString(Resource.String.time));
                string minutes = ts.Minutes.ToString(Resources.GetString(Resource.String.time));
                string seconds = ts.Seconds.ToString(Resources.GetString(Resource.String.time));
                this.RunOnUiThread(() =>
                {
                    if (IsCurrentBookingOngoing)
                        TextViewOngoingCountDown.Text = hour + ":" + minutes + ":" + seconds + " " + Resources.GetString(Resource.String.hour);
                    else
                        TextViewAvailableCountDown.Text = hour + ":" + minutes + ":" + seconds + " " + Resources.GetString(Resource.String.hour);
                    TextViewCurrentTime.Text = DateTime.Now.ToString(Resources.GetString(Resource.String.time_interval_format));
                });
                if (dt < DateTime.Now)
                {
                    _timer.Stop();
                    CurrentBooking = null;
                    AssignBookings();
                }
            }
            catch (Exception exc)
            {
                Crashes.TrackError(exc);
            }
        }
    }

    public class BookingDataModel
    {
        public RoomModel CurrentRoom { get; set; }
        List<BookingDetailModel> BookingList { get; set; }
        static DisplayBookingActivity Activity { get; set; }
        public static BookingDataModel Instance { get; private set; }

        private BookingDataModel(RoomModel currentRoom)
        {
            CurrentRoom = currentRoom;
        }
        public static void Init(RoomModel currentRoom, DisplayBookingActivity activity)
        {
            Instance = new BookingDataModel(currentRoom);
            Activity = activity;
        }
        async public void SetList()
        {
            try
            {
                bool check = false;

                //  Tuple<string, RoomBookingModel> data = await BookingHelper.GetBookings(CurrentRoom.Id);
                //if (!string.IsNullOrEmpty(data.Item1))
                //{
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, data.Item1, ToastLength.Short).Show();
                //    });
                //    return;
                //}
                // RoomBookingModel model = data.Item2;
                //  List<BookingModel> list = model.data;

                List<BookingDetailModel> list = await BookingHelper.GetBookings(CurrentRoom.Id);
                if (BookingList != null && BookingList.Count > 0)
                    check = list.Equals(BookingList);
                BookingList = list;
                if (!check)
                    InvokeSetList?.Invoke(list, EventArgs.Empty);
            }
            catch (Exception exc)
            {

            }
            finally
            {
                Activity.RunOnUiThread(() =>
                {
                    Activity.ProgressBar.Visibility = ViewStates.Gone;
                });
            }
        }
        public static event EventHandler InvokeSetList;
    }

}