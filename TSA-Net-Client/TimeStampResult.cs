using System;
using System.Xml;

namespace TSA_Net_Client
{
    public class TimeStampResult
    {
        public string OriginalContent { get; set; }
        public XmlDocument TimeStampXml { get; set; }
        public string Signature { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? TimeStampDateTime { get; set; }
    }
}
