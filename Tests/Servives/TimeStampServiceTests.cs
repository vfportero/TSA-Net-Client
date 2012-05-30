using System;
using System.Xml;
using NUnit.Framework;
using Rhino.Mocks;
using TSA_Net_Client.Providers;
using TSA_Net_Client.Services;
using Terminis.Tests.Helpers;

namespace Tests.Servives
{
    [TestFixture]
    public class TimeStampServiceTests : BaseTests
    {
        private ITimeStampProviderClient _timeStampProviderClient;
        private TimeStampService _timeStampService;
        private string _content;
        private string _signature;

        [SetUp]
        public void SetUp()
        {
            _timeStampProviderClient = InitializeAndInject<ITimeStampProviderClient>();
            _timeStampProviderClient.Stub(s => s.GenerateTimeStamp(Arg<string>.Is.Anything))
                .Return(new TimeStampProviderApiResult())
                .WhenCalled(a=>
                                {
                                    string signature = "<signature>bla bla</signature>";
                                    var result = new TimeStampProviderApiResult
                                                     {IsSuccess = true, ApiResponse = new XmlDocument()};
                                    result.ApiResponse.LoadXml(signature);
                                    result.Signature = signature;

                                    a.ReturnValue = result;
                                });
            _timeStampProviderClient.Stub(s => s.VerifyTimeStamp(Arg<string>.Is.Anything, Arg<string>.Is.Anything))
               .Return(new TimeStampProviderApiResult())
               .WhenCalled(a =>
               {
                   var result = new TimeStampProviderApiResult();
                   result.IsSuccess = true;
                   result.ApiResponse = new XmlDocument();
                   result.ApiResponse.LoadXml("<response>bla bla</response>");

                   a.ReturnValue = result;
               });

            _timeStampService = new TimeStampService(_timeStampProviderClient);
            _content = "anything";
            _signature = "qwerty";
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_GenerateTimeStamp_Null_Content_Throws_Exception()
        {
            _timeStampService.GenerateTimeStamp(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_GenerateTimeStamp_Empty_Content_Throws_Exception()
        {
            _timeStampService.GenerateTimeStamp(string.Empty);
        }

        [Test]
        public void TimeStampService_GenerateTimeStamp_Returns_TimeStampResult()
        {
            var result = _timeStampService.GenerateTimeStamp(_content);

            Assert.That(result,Is.Not.Null);
            Assert.That(result.IsSuccess,Is.True);
            Assert.That(result.TimeStampXml,Is.Not.Null);
            Assert.That(result.Signature,Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_VerifyTimeStamp_Null_Content_Throws_Exception()
        {
            _timeStampService.VerifyTimeStamp(null, _signature);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_VerifyTimeStamp_Empty_Content_Throws_Exception()
        {
            _timeStampService.VerifyTimeStamp(string.Empty, _signature);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_VerifyTimeStamp_Empty_Signature_Throws_Exception()
        {
            _timeStampService.VerifyTimeStamp(_content, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TimeStampService_VerifyTimeStamp_Null_Signature_Throws_Exception()
        {
            _timeStampService.VerifyTimeStamp(_content, null);
        }

        [Test]
        public void TimeStampService_VerifyTimeStamp_Returns_TimeStampResult()
        {
            var result = _timeStampService.VerifyTimeStamp(_content,_signature);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.TimeStampXml, Is.Not.Null);
            Assert.That(result.Signature, Is.Null);
        }
    }
}
