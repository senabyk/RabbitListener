using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitListener.Application.Interfaces;
using RabbitListener.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Application.Services
{
    public class UrlProcessor : IUrlProcessor
    {
        private readonly ILogger<UrlProcessor> _logger;

        public UrlProcessor(ILogger<UrlProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessUrlAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                    var statusCode = (int)response.StatusCode;

                    LogUrlStatus(url, statusCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing URL: {url}. Error: {ex.Message}");
                }
            }
        }

        private void LogUrlStatus(string url, int statusCode)
        {
            var logEntry = new LogEntry();
            logEntry.StatusCode = (System.Net.HttpStatusCode)statusCode;
            logEntry.Url = url;

            var logEntryJson = JsonConvert.SerializeObject(logEntry);
            _logger.LogInformation(logEntryJson);
        }
    }
}
