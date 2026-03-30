using Moq;
using Nager.Dns.FunctionalTest.Helpers;
using Nager.Dns.Models;

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
        [DataTestMethod]
        public async Task Query_DnsProvider_Test(DnsProvider dnsProvider)
        {
            var loggerMock = LoggerHelper.GetLogger<DnsClient>();
            var httpClientFactory = this.GetHttpClientFactory();

            var dnsQuestions = new DnsQuestion[] { new DnsQuestion("google.com", DnsRecordType.A) };

            IDnsClient dnsClient = new DnsClient(httpClientFactory, loggerMock.Object);
            var responses = await dnsClient.BulkDnsQueryAsync(dnsQuestions, dnsProvider);

            Assert.AreEqual(1, responses.Count, "To much responses");

            var response = responses.First();

            Assert.AreEqual(DnsResponseCode.NoError, (DnsResponseCode)response.Status);
            Assert.IsTrue(response.Answer.Length > 0);
            Assert.AreEqual("google.com", response.Question[0].Name.TrimEnd('.'));
        }
    }
}