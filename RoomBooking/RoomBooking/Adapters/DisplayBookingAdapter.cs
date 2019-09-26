using Android.Content;
using Android.Views;
using Android.Widget;
using RoomBooking.Models;
using System;
using System.Collections.Generic;

namespace RoomBooking.Adapters
{
    class DisplayBookingAdapter : BaseAdapter<BookingDetailModel>
    {
        Context context;
        List<BookingDetailModel> list = new List<BookingDetailModel>();
        public int listHeight = 0;
        int cellHeight = 200;
        public DisplayBookingAdapter(Context context, List<BookingDetailModel> list,int listHeight)
        {
            this.context = context;
            this.list = list;
            this.listHeight = listHeight;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.display_booking_cell_layout, parent, false);
                TextView textViewTimeInterval = view.FindViewById<TextView>(Resource.Id.textViewTimeInterval);
                TextView textViewPurpose= view.FindViewById<TextView>(Resource.Id.textViewPurpose);
                TextView textViewBookedBy = view.FindViewById<TextView>(Resource.Id.textViewBookedBy);
                view.Tag = new DisplayBookingAdapterViewHolder() {TextViewTimeInterval = textViewTimeInterval, TextViewPurpose = textViewPurpose , TextViewBookedBy=textViewBookedBy};
            }
            DisplayBookingAdapterViewHolder holder = (DisplayBookingAdapterViewHolder)view.Tag;
            holder.TextViewTimeInterval.Text =  list[position].start.ToString("HH:mm") + " - " + list[position].end.ToString("HH:mm");
            holder.TextViewPurpose.Text = list[position].purpose;
            holder.TextViewBookedBy.Text = "by " + list[position].reserved_by;
            cellHeight = cellHeight == 0 ? view.LayoutParameters.Height : cellHeight;
            return view;
        }

        public override int Count
        {
            get
            {
                try
                {
                    return list.Count;
                }
               catch(Exception exc)
                {
                    return 0;
                }
            }
        }

        public override BookingDetailModel this[int position]
        {
            get { return list[position]; }
        }
    }

    class DisplayBookingAdapterViewHolder : Java.Lang.Object
    {
        public TextView TextViewTimeInterval { get; set; }
        public TextView TextViewPurpose { get; set; }
        public TextView TextViewBookedBy { get; set; }
    }
}
