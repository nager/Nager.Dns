using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nager.Dns.Models;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DnsClient> _logger;

        /// <summary>
        /// Dns Client over HTTPS
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="logger"></param>
        public DnsClient(
            IHttpClientFactory httpClientFactory,
            ILogger<DnsClient>? logger = default)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger ?? new NullLogger<DnsClient>();
        }

        /// <summary>
        /// Bulk Dns Query
        /// </summary>
        /// <param name="dnsProvider"></param>
        /// <param name="dnsQuestions"></param>
        /// <param name="maxConcurrentRequests"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ReadOnlyCollection<DnsResponse>> BulkDnsQueryAsync(
            DnsProvider dnsProvider,
            IEnumerable<DnsQuestion> dnsQuestions,
            int maxConcurrentRequests = 20,
            CancellationToken cancellationToken = default)
        {
            var totalDnsQuestions = dnsQuestions.Count();

            var dnsResponses = new List<DnsResponse>();

            for (var i = 0; i < totalDnsQuestions; i += maxConcurrentRequests)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var dnsQuestionPackage = dnsQuestions.Skip(i).Take(maxConcurrentRequests);

                dnsResponses.AddRange(await this.QueryDnsQuestions(dnsProvider, dnsQuestionPackage, cancellationToken));
            }

            return dnsResponses.AsReadOnly();
        }

        /// <summary>
        /// Dns Query
        /// </summary>
        /// <param name="dnsProvider"></param>
        /// <param name="dnsQuestion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DnsResponse> DnsQueryAsync(
            DnsProvider dnsProvider,
            DnsQuestion dnsQuestion,
            CancellationToken cancellationToken = default)
        {
            var dnsResponses = await this.QueryDnsQuestions(dnsProvider, [dnsQuestion], cancellationToken);
            return dnsResponses.Single();
        }

        private HttpClient GetHttpClient(DnsProvider dnsProvider)
        {
            var httpClient = this._httpClientFactory.CreateClient();

            switch (dnsProvider)
            {
                case DnsProvider.Google:
                    httpClient.BaseAddress = new Uri("https://dns.google/resolve");
                    break;
                case DnsProvider.Cloudflare:
                    httpClient.BaseAddress = new Uri("https://cloudflare-dns.com/dns-query");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/dns-json");
                    break;
                default:
                    break;
            }

            return httpClient;
        }

        private async Task<IEnumerable<DnsResponse>> QueryDnsQuestions(
            DnsProvider dnsProvider,
            IEnumerable<DnsQuestion> dnsQuestions,
            CancellationToken cancellationToken = default)
        {
            var errors = 0;

            var httpClient = this.GetHttpClient(dnsProvider);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var httpQueryTasks = new List<Task<HttpResponseMessage?>>();
            foreach (var dnsQuestion in dnsQuestions)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var requestUri = $"?name={dnsQuestion.Name}&type={dnsQuestion.Type}";
                httpQueryTasks.Add(httpClient.GetAsync(requestUri, cancellationToken).ContinueWith(completedTask =>
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

                var jsonReadTask = httpResponseMessage.Content.ReadFromJsonAsync<DnsResponse>(cancellationToken).ContinueWith((completedTask) =>
                {
                    try
                    {
                        if (completedTask.IsFaulted)
                        {
                            return null;
                        }
                        else if (completedTask.IsCanceled)
                        {
                            return null;
                        }
                    }
                    finally
                    {
                        httpResponseMessage.Dispose();
                    }

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
                .Select(o => o.Result!);
        }
    }
}
