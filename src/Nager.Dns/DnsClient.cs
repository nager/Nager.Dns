using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nager.Dns.Models;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Nager.Dns
{
    /// <summary>
    /// Dns Client over HTTPS
    /// </summary>
    public class DnsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DnsClient> _logger;

        /// <summary>
        /// Dns Client over HTTPS
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        public DnsClient(
            HttpClient httpClient,
            ILogger<DnsClient>? logger = default)
        {
            this._httpClient = httpClient;
            this._logger = logger ?? new NullLogger<DnsClient>();
        }

        public async Task<IEnumerable<DnsResponse>> MultiQueryAsync(
            IEnumerable<DnsQuestion> dnsQuestions,
            int maxConcurrentRequests = 20)
        {
            var totalDnsQuestions = dnsQuestions.Count();

            var dnsResponses = new List<DnsResponse>();

            for (int i = 0; i < totalDnsQuestions; i += maxConcurrentRequests)
            {
                var dnsQuestionPackage = dnsQuestions.Skip(i).Take(maxConcurrentRequests);

                dnsResponses.AddRange(await this.QueryDnsQuestions(dnsQuestionPackage));
            }

            return dnsResponses;
        }

        private async Task<ReadOnlyCollection<DnsResponse>> QueryDnsQuestions(IEnumerable<DnsQuestion> dnsQuestions)
        {
            var errors = 0;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var httpQueryTasks = new List<Task<HttpResponseMessage?>>();
            foreach (var dnsQuestion in dnsQuestions)
            {
                var requestUri = $"https://dns.google/resolve?name={dnsQuestion.Name}&type={dnsQuestion.Type}";
                httpQueryTasks.Add(this._httpClient.GetAsync(requestUri).ContinueWith(completedTask =>
                {
                    if (completedTask.IsFaulted)
                    {
                        return null;
                    }
                    else if (completedTask.IsCanceled)
                    {
                        return null;
                    }

                    return completedTask.Result;
                }));
            }

            await Task.WhenAll(httpQueryTasks);

            var jsonReadTasks = new List<Task<DnsResponse?>>();
            foreach (var queryTask in httpQueryTasks)
            {
                var httpResponseMessage = queryTask.Result;

                if (httpResponseMessage == null)
                {
                    errors++;
                    continue;
                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    errors++;
                    httpResponseMessage.Dispose();
                    continue;
                }

                var jsonReadTask = httpResponseMessage.Content.ReadFromJsonAsync<DnsResponse>().ContinueWith((completedTask) =>
                {
                    if (completedTask.IsFaulted)
                    {
                        return null;
                    }
                    else if (completedTask.IsCanceled)
                    {
                        return null;
                    }

                    httpResponseMessage.Dispose();

                    return completedTask.Result;
                });

                jsonReadTasks.Add(jsonReadTask);
            }

            await Task.WhenAll(jsonReadTasks);

            stopwatch.Stop();
            errors += jsonReadTasks.Where(o => o.Result == null).Count();

            if (this._logger.IsEnabled(LogLevel.Debug))
            {
                this._logger.LogDebug($"{nameof(QueryDnsQuestions)} {(stopwatch.Elapsed.TotalMilliseconds / dnsQuestions.Count()):0.00}ms, errors:{errors}");
            }

            return jsonReadTasks
                .Where(o => o.Result != null)
                .Select(o => o.Result!)
                .ToList()
                .AsReadOnly();
        }
    }
}
