using Microsoft.Extensions.Logging;
using Moq;
using RabbitListener.Application.Services;
using System.Net;

namespace RabbitListener.Tests
{
    public class UrlProcessorTests
    {
        [Fact]
        public async Task ProcessUrlAsync_ValidUrl_LogsCorrectStatusCode()
        {
            var url = "https://example.com";
            var expectedStatusCode = HttpStatusCode.OK;

            var loggerMock = new Mock<ILogger<UrlProcessor>>();
            var httpClientMock = new Mock<HttpClient>();

            httpClientMock
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = expectedStatusCode
                });

            var urlProcessor = new UrlProcessor(loggerMock.Object);

            // Act
            await urlProcessor.ProcessUrlAsync(url);

            // Assert
            loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => ContainsUrlAndStatusCode(o, url, expectedStatusCode)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once);
        }

        private static bool ContainsUrlAndStatusCode(object state, string url, HttpStatusCode statusCode)
        {
            if (state is IReadOnlyList<KeyValuePair<string, object>> properties)
            {
                var logMessage = properties.FirstOrDefault(p => p.Key == "{OriginalFormat}").Value?.ToString();
                return logMessage?.Contains(url) == true && logMessage.Contains(((int)statusCode).ToString());
            }
            return false;
        }


    }
}