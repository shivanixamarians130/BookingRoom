using RestSharp;
using RoomBooking.Models;
using System;
using System.Threading.Tasks;

namespace RoomBooking.Helpers
{
    class APIHelper
    {
        async public static Task<Tuple<string,string>> GetAsync(string endpoint)
        {
            if (await Connectivity.IsInternetAvailable())
            {
                RestClient restClient = new RestClient(ConstantHelper.BaseUrl);
                var taskCompletionSource = new TaskCompletionSource<IRestResponse>();
                try
                {
                    RestRequest mRestRequest = new RestRequest(endpoint, Method.GET);
                    restClient.ExecuteAsync(mRestRequest, restResponse =>
                    {
                        if (restResponse.ErrorException != null)
                        {
                            const string message = "Error retrieving response.";
                        }
                        taskCompletionSource.SetResult(restResponse);
                    });

                    var response = await taskCompletionSource.Task;
                    var statusCode = (int)response.StatusCode;
                    if (statusCode == 200)
                    {
                        return new Tuple<string, string>(response.Content, "");
                    }
                    return new Tuple<string, string> (null,response.ErrorMessage);
                }

                catch (Exception exc)
                {
                    return new Tuple<string, string>(null,exc.Message);
                }
            }
            else
            {
                return new Tuple<string, string>(null, "No nertwork connection");
            }

        }



    }
}