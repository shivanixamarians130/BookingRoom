using Android.Content;
using Newtonsoft.Json;
using RoomBooking.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Helpers
{
    class BookingHelper
    {
        async public static Task<Tuple<string, RoomBookingModel>> GetBookings(int selectedRoomId, Context context = null)
        {
            string endPoint = ConstantHelper.AllBookings + "?" + "roomId" + "=" + selectedRoomId + "&" + "limit" + "=" + ConstantHelper.NumberOfBookingsPerPage;
            Tuple<string, string> data = await APIHelper.GetAsync(endPoint);
            string responseContent = data.Item1;
            if (responseContent != null)
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    RoomBookingModel list = JsonConvert.DeserializeObject<RoomBookingModel>(responseContent);
                    return new Tuple<string, RoomBookingModel>(null, list);
                }
            }
            return new Tuple<string, RoomBookingModel>(data.Item2, null);

        }

    //    async public static Task<List<BookingDetailModel>> GetBookings(int selectedRoomId, Context context = null)
    //    {
    //        //string endPoint = ConstantHelper.AllBookings + "?" + "roomId" + "=" + selectedRoomId + "&" + "limit" + "=" + ConstantHelper.NumberOfBookingsPerPage;
    //        //Tuple<string, string> data = await APIHelper.GetAsync(endPoint);
    //        //string responseContent = data.Item1;
    //        //if (responseContent != null)
    //        //{
    //        //    if (!string.IsNullOrEmpty(responseContent))
    //        //    {
    //        //        RoomBookingModel list = JsonConvert.DeserializeObject<RoomBookingModel>(responseContent);
    //        //        return new Tuple<string, RoomBookingModel>(null, list);
    //        //    }
    //        //}
    //        //return new Tuple<string, RoomBookingModel>(data.Item2, null);
    //        List<BookingDetailModel> MeetingList = new List<BookingDetailModel>();
    //        await Task.Delay(1000);
    //        //    if (MeetingList.Count > 0)
    //        //      return MeetingList;
    //        MeetingList.Clear();
    //        MeetingList.Add(new BookingDetailModel() { start = DateTime.Now, end = DateTime.Now.AddMinutes(1), purpose = "Xamarin1" });
    //        //  MeetingList.Add(new BookingModel() { start = DateTime.Now.AddMinutes(1), end = DateTime.Now.AddMinutes(2), purpose = "Xamarin2" });
    //        MeetingList.Add(new BookingDetailModel() { start = DateTime.Now.AddMinutes(2), end = DateTime.Now.AddMinutes(3), purpose = "Xamarin3" });
    //        MeetingList.Add(new BookingDetailModel() { start = DateTime.Now.AddMinutes(17), end = DateTime.Now.AddMinutes(20), purpose = "Xamarin5" });
    //        MeetingList.Add(new BookingDetailModel() { start = DateTime.Now.AddMinutes(20), end = DateTime.Now.AddMinutes(30), purpose = "Xamarin6" });
    //        return MeetingList;
    //    }
    //}
}
