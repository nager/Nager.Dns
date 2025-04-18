using Moq;
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
        [DataTestMethod]
        public async Task Query_Google_Test(DnsProvider dnsProvider)
        {
            var httpClientFactory = this.GetHttpClientFactory();

            IDnsClient dnsClient = new DnsClient(httpClientFactory);
            var responses = await dnsClient.BulkDnsQueryAsync([new DnsQuestion("google.com", DnsRecordType.A)], dnsProvider);

            Assert.AreEqual(1, responses.Count, "To much responses");

            var response = responses.First();

            Assert.AreEqual(DnsResponseCode.NoError, (DnsResponseCode)response.Status);
            Assert.IsTrue(response.Answer.Length > 0);
        }
    }
}