using System;
using TSA_Net_Client.Interfaces;
using TSA_Net_Client.Providers;

namespace TSA_Net_Client.Services
{
    public class TimeStampService : ITimeStampService
    {
        private readonly ITimeStampProviderClient _timeStampClient;

        public TimeStampService(ITimeStampProviderClient timeStampClient)
        {
            _timeStampClient = timeStampClient;
        }

        public TimeStampResult GetTimeStamp(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }
            TimeStampResult result = new TimeStampResult { IsSuccess = false };
            var apiResult = _timeStampClient.GenerateTimeStamp(content);

            if (apiResult.IsSuccess)
            {
                result.IsSuccess = true;
                result.TimeStampXml = apiResult.ApiResponse;
                result.Signature = apiResult.Signature;
                result.TimeStampDateTime = apiResult.TimeStampDateTime;
            }

            return result;
        }

        public TimeStampResult VerifyTimeStamp(string content, string signature)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }
            if (string.IsNullOrEmpty(signature))
            {
                throw new ArgumentNullException("signature");
            }

            TimeStampResult result = new TimeStampResult { IsSuccess = false };
            var apiResult = _timeStampClient.VerifyTimeStamp(content, signature);

            if (apiResult.IsSuccess)
            {
                result.IsSuccess = true;
                result.TimeStampXml = apiResult.ApiResponse;
            }

            return result;
        }
    }
}
