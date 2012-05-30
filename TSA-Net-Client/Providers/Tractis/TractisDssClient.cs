using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using TSA_Net_Client.Providers.Helpers;

namespace TSA_Net_Client.Providers.Tractis
{
    public class TractisDssClient : ITimeStampProviderClient
    {
        public TractisDssClientProperties TractisDssClientProperties { get; set; }

        public TractisDssClient()
        {
            TractisDssClientProperties = new TractisDssClientProperties();
        }

        public TimeStampProviderApiResult GenerateTimeStamp(string contentToStamp)
        {
            if (string.IsNullOrEmpty(contentToStamp))
            {
                throw new ArgumentNullException("contentToStamp");
            }

            TimeStampProviderApiResult result = new TimeStampProviderApiResult { IsSuccess = false };

            string dataXml = string.Format(RequestTemplates.StampRequest, HashHelper.EncodeTo64(contentToStamp));

            string apiResponse = SendRequest(dataXml, TractisDssClientProperties.ApiTimeStampUrl);

            if (!string.IsNullOrEmpty(apiResponse))
            {
                result.ApiResponse = new XmlDocument();
                result.ApiResponse.LoadXml(apiResponse);

                XmlNamespaceManager tractisDssNamespace = new XmlNamespaceManager(result.ApiResponse.NameTable);
                tractisDssNamespace.AddNamespace("dss", "urn:oasis:names:tc:dss:1.0:core:schema");
                tractisDssNamespace.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                tractisDssNamespace.AddNamespace("ns3", "urn:oasis:names:tc:dss:1.0:core:schema");

                result.IsSuccess =
                    result.ApiResponse.SelectSingleNode("//dss:ResultMajor", tractisDssNamespace).InnerText.ToLower().Contains("success");
                result.Signature = ExtractSignatureString(apiResponse);
                result.TimeStampDateTime = DateTime.Parse(result.ApiResponse.SelectSingleNode("//ns3:SignResponse/ns3:OptionalOutputs/ns3:SigningTimeInfo/ns3:SigningTime", tractisDssNamespace).InnerText);
                result.OriginalContent = contentToStamp;

            }

            return result;


        }

        private string ExtractSignatureString(string apiResponse)
        {
            int signatureStartPosition = apiResponse.IndexOf("<ds:Signature");
            int signatureEndPosition = apiResponse.IndexOf("</ns3:SignatureObject>");
            return apiResponse.Substring(signatureStartPosition, apiResponse.Length - signatureStartPosition - (apiResponse.Length - signatureEndPosition));
        }

        public TimeStampProviderApiResult VerifyTimeStamp(string contentToVerify, string signature)
        {
            if (string.IsNullOrEmpty(contentToVerify))
            {
                throw new ArgumentNullException("contentToVerify");
            }
            if (string.IsNullOrEmpty(signature))
            {
                throw new ArgumentNullException("signature");
            }

            TimeStampProviderApiResult result = new TimeStampProviderApiResult { IsSuccess = false };

            string dataXml = string.Format(RequestTemplates.VerifyRequest, HashHelper.EncodeTo64(HashHelper.HashString(contentToVerify)), signature);

            string apiResponse = SendRequest(dataXml, TractisDssClientProperties.ApiVerifyUrl);

            if (!string.IsNullOrEmpty(apiResponse))
            {
                result.ApiResponse = new XmlDocument();
                result.ApiResponse.LoadXml(apiResponse);

                XmlNamespaceManager tractisDssNamespace = new XmlNamespaceManager(result.ApiResponse.NameTable);
                tractisDssNamespace.AddNamespace("dss", "urn:oasis:names:tc:dss:1.0:core:schema");
                tractisDssNamespace.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                result.IsSuccess = result.ApiResponse.SelectSingleNode("//dss:ResultMajor", tractisDssNamespace).InnerText.ToLower().Contains("success") &&
                    result.ApiResponse.SelectSingleNode("//dss:ResultMinor", tractisDssNamespace).InnerText.ToLower().Contains("onalldocuments");
                result.OriginalContent = contentToVerify;
            }
            return result;

        }

        private string SendRequest(string dataToSend, string url)
        {
            string result = string.Empty;
            Stream responseStream = null;
            StreamReader reader = null;
            WebResponse response = null;
            var httpWebRequest = CreateHttpWebRequest(dataToSend, url);

            try
            {
                response = httpWebRequest.GetResponse();
                responseStream = response.GetResponseStream();

                reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return result;
        }

        private HttpWebRequest CreateHttpWebRequest(string dataToSend, string url)
        {
            byte[] dataXmlByteArray = HashHelper.StrToByteArray(dataToSend);

            WebRequest request = WebRequest.Create(url);


            var httpWebRequest = request as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "*/*";

                string credentials = String.Format("{0}:{1}", TractisDssClientProperties.ApiUserName, TractisDssClientProperties.ApiPassword);
                byte[] bytes = Encoding.ASCII.GetBytes(credentials);
                string base64 = Convert.ToBase64String(bytes);
                string authorization = String.Concat("Basic ", base64);
                request.Headers.Add("Authorization", authorization);

                httpWebRequest.ContentLength = dataXmlByteArray.Length;
                Stream dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(dataXmlByteArray, 0, dataXmlByteArray.Length);
                dataStream.Close();
            }
            return httpWebRequest;
        }
    }
}
