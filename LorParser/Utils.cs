using System.Net.Http;
using System.Threading.Tasks;

namespace LorParser
{
    public static class Utils
    {
        private static readonly HttpClient _HttpClient = new HttpClient();

        public static async Task<string> GetRequest(string uri)
        {
            return await Task.Run(async () => { return await _HttpClient.GetStringAsync(uri); });
        }
    }
}