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

            Assert.AreEqual(1, responses.Count);

            var response = responses.First();

            Assert.AreEqual(DnsResponseStatus.NoError, (DnsResponseStatus)response.Status);
            Assert.AreEqual(1, response.Answer.Length);
        }

        [TestMethod]
        public async Task Cloudflare_Test()
        {
            var httpClientFactory = this.GetHttpClientFactory();

            var dnsClient = new DnsClient(httpClientFactory);
            var responses = await dnsClient.MultiQueryAsync(DnsProvider.Cloudflare, [new DnsQuestion("google.com", DnsAnswerType.A)]);

            Assert.AreEqual(1, responses.Count);

            var response = responses.First();

            Assert.AreEqual(DnsResponseStatus.NoError, (DnsResponseStatus)response.Status);
        }
    }
}