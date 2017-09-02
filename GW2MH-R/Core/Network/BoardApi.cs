using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GW2MH.Core.Network
{
    internal static class BoardApi
    {

        internal static async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var request = WebRequest.Create(string.Format("http://yothri.com/board/custom_api/login.php?email={0}&password={1}", username, password));
                request.Method = "GET";

                var response = await request.GetResponseAsync();
                var text = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(text);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}