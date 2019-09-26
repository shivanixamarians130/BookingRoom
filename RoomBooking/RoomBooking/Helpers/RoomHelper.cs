using Android.Content;
using Newtonsoft.Json;
using RoomBooking.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Helpers
{
    class RoomHelper
    {
       async public static Task<Tuple<string,List<RoomModel>>> GetRoomsAsync(Context context)
        {
            Tuple<string,string> data = await APIHelper.GetAsync(ConstantHelper.AllRooms);
            string responseContent = data.Item1;
            if(responseContent != null)
            {
                if (!string.IsNullOrEmpty(responseContent))
                {
                    List<RoomModel> roomList = JsonConvert.DeserializeObject<List<RoomModel>>(responseContent);
                    return new Tuple<string, List<RoomModel>> ("",roomList);
                }
            }
            return new Tuple<string, List<RoomModel>>(data.Item2, null);

        }
    }
}