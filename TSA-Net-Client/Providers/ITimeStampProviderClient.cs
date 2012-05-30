namespace TSA_Net_Client.Providers
{
    public interface ITimeStampProviderClient
    {
        TimeStampProviderApiResult GenerateTimeStamp(string contentToStamp);
        TimeStampProviderApiResult VerifyTimeStamp(string contentToVerify, string signature);
    }
}