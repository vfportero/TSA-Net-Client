using System;
using System.Security.Cryptography;
using System.Text;

namespace TSA_Net_Client.Providers.Helpers
{
    public static class HashHelper
    {
        public static string EncodeTo64(string toEncode)
        {
            if (string.IsNullOrEmpty(toEncode))
            {
                throw new ArgumentNullException("toEncode");
            }
            return EncodeTo64(Encoding.UTF8.GetBytes(toEncode));
        }

        public static string EncodeTo64(byte[] toEncode)
        {
            if (toEncode==null)
            {
                throw new ArgumentNullException("toEncode");
            }

            string returnValue
                  = Convert.ToBase64String(toEncode);
            return returnValue;
        }

        public static string DecodeFrom64(string toDecode)
        {
            if (string.IsNullOrEmpty(toDecode))
            {
                throw new ArgumentNullException("toDecode");
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(toDecode));
        }

        public static byte[] StrToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] bytesToConvert)
        {
            if (bytesToConvert == null)
            {
                throw new ArgumentNullException("bytesToConvert");
            }
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(bytesToConvert);
        }

        public static byte[] HashString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            return stream;
        }
    }
}
