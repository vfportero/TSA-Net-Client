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
            ApiUserName = "b5f7c9d1-93b5-4343-a252-6a3e4da4dce7";
            ApiPassword = "6036975f8ec4d8b7530ebde7cb010e0d83cf376b";
        }
    }
}
