using NUnit.Framework;
using TSA_Net_Client.Providers.Tractis;

namespace Tests.Integration
{
    [TestFixture]
    public class TractisDssClientIntegrationTests : BaseTests
    {
        private TractisDssClient _client;
        private string _contentToTest;

        [SetUp]
        public void SetUp()
        {
            _client = new TractisDssClient();
            _contentToTest = "text to be timestamped";
        }

        [Test]
        public void GenerateTimeStamp_Generates_New_TimeStamp_And_Verify_It()
        {
            var result = _client.GenerateTimeStamp(_contentToTest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.ApiResponse, Is.Not.Null);
            Assert.That(result.Signature,Is.Not.Null);
            Assert.That(result.Signature,Is.Not.EqualTo(string.Empty));
            Assert.That(result.TimeStampDateTime.HasValue, Is.True);

            var verifiyingResult = _client.VerifyTimeStamp(_contentToTest, result.Signature);

            Assert.That(verifiyingResult, Is.Not.Null);
            Assert.That(verifiyingResult.IsSuccess, Is.True);
            Assert.That(verifiyingResult.ApiResponse, Is.Not.Null);
            Assert.That(verifiyingResult.Signature, Is.Null);
            Assert.That(verifiyingResult.TimeStampDateTime.HasValue, Is.False);

        }
    }
}
