using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GW2MH.Core.Network
{
    internal static class PaymentApi
    {

        internal static async Task<PaymentResponse> CreatePayment(string username)
        {
            PaymentResponse paymentResponse = null;

            try
            {
                using (var client = new HttpClient())
                using (var response = await client.GetAsync("http://api.yothri.com/gw2mh-r/payment.php?action=create&username=" + username))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(result);
                }

                return paymentResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static async Task<bool> PaymentDetails(string username)
        {
            PaymentResponse paymentResponse = null;

            try
            {
                using (var client = new HttpClient())
                using (var response = await client.GetAsync("http://api.yothri.com/gw2mh-r/payment.php?action=paymentDetails&username=" + username))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(result);
                }

                return paymentResponse.success && paymentResponse.executed;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}