using System;
using System.Net;
using System.Threading.Tasks;

namespace RoomBooking.Helpers
{
    public static class Connectivity
    {
        public static Task<bool> IsInternetAvailable()
        {
            try
            {
                using (var client = new MyWebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return Task.FromResult(true);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("hello the error is");
                Console.WriteLine(exc);
                return Task.FromResult(false);
            }
        }

    }


    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 20000;
            return w;
        }
    }
}
