using Android.Content;
using Android.Views;
using Android.Widget;
using RoomBooking.Models;
using System.Collections.Generic;

namespace RoomBooking.Adapters
{
    class RoomAdapter : BaseAdapter<RoomModel>
    {

        Context context;
        List<RoomModel> RoomList;

        public RoomAdapter(Context context,List<RoomModel> roomList)
        {
            this.context = context;
            RoomList = roomList;
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
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.room_cell_layout, parent, false);
                TextView textViewRoomName = view.FindViewById<TextView>(Resource.Id.textViewRoomName);
                view.Tag = new RoomAdapterViewHolder() {TextViewRoomName = textViewRoomName };
            }

            RoomAdapterViewHolder holder = (RoomAdapterViewHolder)view.Tag;
            holder.TextViewRoomName.Text = RoomList[position].Name;
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return RoomList != null ? RoomList.Count : 0;
            }
        }
        public override RoomModel this[int position]
        {
            get { return RoomList[position]; }
        }
    }

    class RoomAdapterViewHolder : Java.Lang.Object
    {
       public TextView TextViewRoomName { get; set; }
    }
}