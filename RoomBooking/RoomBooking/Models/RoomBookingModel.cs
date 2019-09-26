using System;
using System.Collections.Generic;

namespace RoomBooking.Models
{
    public class BookingDetailModel
    {
        public string purpose { get; set; }
        public int room_id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string reserved_by { get; set; }
    }

    public class RoomBookingModel
    {
        public string total { get; set; }
        public string per_page { get; set; }
        public string current_page { get; set; }
        public string last_page { get; set; }
        public object next_page_url { get; set; }
        public object prev_page_url { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public List<BookingDetailModel> data { get; set; }
    }
}