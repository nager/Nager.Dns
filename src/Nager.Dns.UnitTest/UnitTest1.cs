using Nager.Dns.Models;

namespace Nager.Dns.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var dnsClient = new DnsClient(new HttpClient());
            var responses = await dnsClient.MultiQueryAsync([new DnsQuestion("google.com", DnsAnswerType.A)]);

            Assert.AreEqual(DnsResponseStatus.NoError, (DnsResponseStatus)responses.First().Status);
        }
    }
}