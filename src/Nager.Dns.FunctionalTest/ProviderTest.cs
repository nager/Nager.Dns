using Moq;
using Nager.Dns.FunctionalTest.Helpers;
using Nager.Dns.Models;
using System.Net;

namespace Nager.Dns.FunctionalTest
{
    [TestClass]
    public class ProviderTest
    {
        private IHttpClientFactory GetHttpClientFactory()
        {
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            return mockHttpClientFactory.Object;
        }

        [DataRow(DnsProvider.Google)]
        [DataRow(DnsProvider.Cloudflare)]
        [DataRow(DnsProvider.Adguard)]
        [TestMethod]
        public async Task Query_DnsProvider_Test(DnsProvider dnsProvider)
        {
            var loggerMock = LoggerHelper.GetLogger<DnsClient>();
            var httpClientFactory = this.GetHttpClientFactory();

            var dnsQuestions = new DnsQuestion[] { new DnsQuestion("google.com", DnsRecordType.A) };

            IDnsClient dnsClient = new DnsClient(httpClientFactory, loggerMock.Object);
            var responses = await dnsClient.BulkDnsQueryAsync(dnsQuestions, dnsProvider);

            Assert.HasCount(1, responses, "To much responses");

            var response = responses.First();

            Assert.AreEqual(DnsResponseCode.NoError, (DnsResponseCode)response.Status);

            Assert.AreEqual("google.com", response.Question[0].Name.TrimEnd('.'));

            Assert.IsGreaterThan(0, response.Answer.Length);
            Assert.IsNotNull(response.Answer[0].Name);
            Assert.AreEqual("google.com", response.Answer[0].Name.TrimEnd('.'));
            Assert.IsNotNull(response.Answer[0].Data);
            Assert.IsTrue(IPAddress.TryParse(response.Answer[0].Data, out _));
        }
    }
}