using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RoomBooking.Fragments
{
    public class BookingListItemFragment : Fragment
    {
        private TextView TextViewTimePeriod;
        private TextView TextViewPurpose;
        private TextView TextViewReservedBy;

        private string _timePeriod;
        private string _purpose;
        private string _reservedBy;

       public BookingListItemFragment()
        {

        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.display_booking_cell_layout, null);
            if (view != null)
            {
                TextViewTimePeriod = ((TextView)view.FindViewById(Resource.Id.textViewTimeInterval));
                TextViewPurpose = ((TextView)view.FindViewById(Resource.Id.textViewPurpose));
                TextViewReservedBy = ((TextView)view.FindViewById(Resource.Id.textViewBookedBy));
            }
            return view;
        }
        
    }
}