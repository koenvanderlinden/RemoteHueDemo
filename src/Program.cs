using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RemoteHueDemo
{
  class Program
  {
    private static string _remoteApiBase = "https://www.meethue.com/api/";
    private static string _token = "YOUR ACCESS TOKEN"; //Get it from https://www.meethue.com/en-us/user/apps --> url of De-activate link containt access token.

    public static Uri RemoteMessageUrl
    {
      get
      {
        return new Uri(string.Format("{0}{1}?token={2}", _remoteApiBase, "sendmessage", HttpUtility.UrlEncode(_token)));
      }
    }

    static void Main(string[] args)
    {
      EnableLight1Demo();
    }

    private static void EnableLight1Demo()
    {
      dynamic command = new ExpandoObject();
      command.clipCommand = new
      {
        //change state of light 1
        url = "/api/0/lights/1/state",
        method = "PUT",
        body = new
        {
          //set the light state changes
          on = true
        }
      };

      SendMessage(command).Wait();
    }

    private static async Task SendMessage(dynamic command)
    {
      string jsonMessage = JsonConvert.SerializeObject(command);

      HttpClient client = new HttpClient();      
      HttpContent content = new StringContent("clipmessage=" + jsonMessage, Encoding.UTF8, "application/x-www-form-urlencoded");

      var result = await client.PostAsync(RemoteMessageUrl, content).ConfigureAwait(false);
      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
      //success will show: "{\"code\":200,\"message\":\"ok\",\"result\":\"ok\"}"
      //failure could be like: "{\"code\":109,\"message\":\"I don\\u0027t know that token.\",\"result\":\"error\"}"
      //or "{\"code\":113,\"message\":\"Invalid JSON.\",\"result\":\"error\"}"
    }




  }
}
