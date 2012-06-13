using System;
using System.Xml;

namespace TSA_Net_Client.Providers
{
    public class TimeStampProviderApiResult
    {
        public string OriginalContent { get; set; }
        public XmlDocument ApiResponse { get; set; }
        public bool IsSuccess { get; set; }
        public string Signature { get; set; }
        public DateTime? TimeStampDateTime { get; set; }
        public DateTime? TimeStampExpirationDateTime { get; set; }
    }
}
