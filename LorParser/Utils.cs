using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
namespace LorParser
{
    public static  class Utils
    {
        private static HttpClient _HttpClient = new HttpClient();
        public static async Task<string> GetRequest(string uri)
        {
          return await Task.Run(async() =>
          {
              return await _HttpClient.GetStringAsync(uri);
          });
        }
        

    }
}
