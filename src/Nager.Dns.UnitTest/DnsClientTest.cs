using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Nager.Dns.Models;
using System.Net;
using System.Text;

namespace Nager.Dns.UnitTest
{
    [TestClass]
    public class DnsClientTest
    {
        [TestMethod]
        public async Task QuerySingle_ShouldReturnValidResponse()
        {
            var mockLogger = new Mock<ILogger<DnsClient>>();

            var getRepsonseContent = @"{""Status"":0,""TC"":false,""RD"":true,""RA"":true,""AD"":false,""CD"":false,""Question"":[{""name"":""google.at."",""type"":1}],""Answer"":[{""name"":""google.at."",""type"":1,""TTL"":230,""data"":""172.217.16.163""}]}";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(getRepsonseContent, Encoding.UTF8, "application/json")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var client = new DnsClient(mockFactory.Object, mockLogger.Object);

            Assert.IsNotNull(client);

            var response = await client.DnsQueryAsync(new DnsQuestion
            {
                Name = "google.com",
                Type = DnsRecordType.A
            });

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Answer.Length > 0);
            Assert.AreEqual("172.217.16.163", response.Answer[0].Data);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.AbsoluteUri.Contains("google.com")),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}