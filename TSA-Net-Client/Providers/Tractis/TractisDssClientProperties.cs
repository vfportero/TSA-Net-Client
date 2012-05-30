namespace TSA_Net_Client.Providers.Tractis
{
    public class TractisDssClientProperties
    {
        public string ApiTimeStampUrl { get; set; }
        public string ApiVerifyUrl { get; set; }
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }

        public TractisDssClientProperties()
        {
            ApiTimeStampUrl = "https://api.tractis.com/tsa";
            ApiVerifyUrl = "https://api.tractis.com/sva";
            ApiUserName = "your Tractis ApiKey ID here";
            ApiPassword = "your Tractis ApiKey Secret here";
        }
    }
}
