namespace GW2MH.Core.Network
{
    public class PaymentResponse
    {

        public bool success { get; set; }
        public string error_message { get; set; }
        public string message { get; set; }
        public string paymentId { get; set; }
        public string approvalLink { get; set; }
        public bool executed { get; set; }

    }
}