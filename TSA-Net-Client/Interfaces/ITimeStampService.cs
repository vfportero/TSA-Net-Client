namespace TSA_Net_Client.Interfaces
{
    public interface ITimeStampService
    {
        TimeStampResult GetTimeStamp(string content);
        TimeStampResult VerifyTimeStamp(string content,string signature);
    }
}