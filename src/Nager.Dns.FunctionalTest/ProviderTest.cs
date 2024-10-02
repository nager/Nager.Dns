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

        [TestMethod]
        public async Task Google_Test()
        {
            var httpClientFactory = this.GetHttpClientFactory();

            var dnsClient = new DnsClient(httpClientFactory);
            var responses = await dnsClient.MultiQueryAsync(DnsProvider.Google, [new DnsQuestion("google.com", DnsAnswerType.A)]);

            Assert.AreEqual(1, responses.Count, "To much responses");

            var response = responses.First();

            Assert.AreEqual(DnsResponseStatus.NoError, (DnsResponseStatus)response.Status);
            Assert.IsTrue(response.Answer.Length > 0);
        }

        [TestMethod]
        public async Task Cloudflare_Test()
        {
            var httpClientFactory = this.GetHttpClientFactory();

            var dnsClient = new DnsClient(httpClientFactory);
            var responses = await dnsClient.MultiQueryAsync(DnsProvider.Cloudflare, [new DnsQuestion("google.com", DnsAnswerType.A)]);

            Assert.AreEqual(1, responses.Count, "To much responses");

            var response = responses.First();

            Assert.AreEqual(DnsResponseStatus.NoError, (DnsResponseStatus)response.Status);
            Assert.IsTrue(response.Answer.Length > 0);
        }
    }
}