using NUnit.Framework;
using StructureMap;
using TSA_Net_Client.Providers;
using TSA_Net_Client.Providers.Tractis;
using TSA_Net_Client.Services;
using Terminis.Tests.Helpers;

namespace Tests.Integration
{
    [TestFixture]
    public class TimeStampServiceIntegrationTests : BaseTests
    {
        private ITimeStampProviderClient _timeStampProviderClient;
        private TimeStampService _timeStampService;
        private string _content;
        private string _timestampSignature;

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Configure(c => c.For<ITimeStampProviderClient>().Use<TractisDssClient>());
            _timeStampProviderClient = ObjectFactory.GetInstance<ITimeStampProviderClient>();
            _timeStampService = new TimeStampService(_timeStampProviderClient);
            _content = "anything";
            _timestampSignature = "incorrect signature";
        }

        [Test]
        public void Service_GetTimeStamp_Returns_TimeStamp()
        {
            var result = _timeStampService.GenerateTimeStamp(_content);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.TimeStampXml, Is.Not.Null);
            Assert.That(result.TimeStampDateTime.HasValue, Is.True);
        }

        [Test]
        public void Service_VerifyTimeStamp_Returns_False_If_Signature_Is_Invalid()
        {
            var result = _timeStampService.VerifyTimeStamp(_content, _timestampSignature);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.TimeStampXml, Is.Null);
            Assert.That(result.Signature, Is.Null);
            Assert.That(result.TimeStampDateTime.HasValue, Is.False);

        }
    }
}
