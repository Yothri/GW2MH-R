namespace GW2MH.Core.Network
{
    public class LoginResponse
    {

        public string status { get; set; }
        public string msg { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string[] group_data { get; set; }

    }
}