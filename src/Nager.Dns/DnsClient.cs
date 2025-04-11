using Microsoft.Extensions.DependencyInjection;
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
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly HttpClient? _httpClient;
        private readonly ILogger<DnsClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory to create HTTP clients.</param>
        /// <param name="logger">The logger instance for diagnostic messages. Defaults to a no-op logger.</param>
        [ActivatorUtilitiesConstructor]
        public DnsClient(
            IHttpClientFactory httpClientFactory,
            ILogger<DnsClient>? logger = default)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger ?? new NullLogger<DnsClient>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsClient"/> class.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance used to send DNS queries over HTTPS.</param>
        /// <param name="logger">The logger instance for diagnostic messages. Defaults to a no-op logger.</param>
        public DnsClient(
            HttpClient httpClient,
            ILogger<DnsClient>? logger = default)
        {
            this._httpClient = httpClient;
            this._logger = logger ?? new NullLogger<DnsClient>();
        }

        /// <summary>
        /// Performs a bulk DNS query for multiple questions, with optional concurrency control.
        /// </summary>
        /// <param name="dnsQuestions">The list of DNS questions to resolve.</param>
        /// <param name="dnsProvider">The DNS provider to use. Default to <see cref="DnsProvider.Google"/></param>
        /// <param name="maxConcurrentRequests">The maximum number of concurrent requests. Defaults to 20.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns></returns>
        public async Task<ReadOnlyCollection<DnsResponse>> BulkDnsQueryAsync(
            IEnumerable<DnsQuestion> dnsQuestions,
            DnsProvider dnsProvider = DnsProvider.Google,
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
        /// Performs a DNS query for a single question.
        /// </summary>
        /// <param name="dnsQuestion">The DNS question to resolve.</param>
        /// <param name="dnsProvider">The DNS provider to use. Default to <see cref="DnsProvider.Google"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns></returns>
        public async Task<DnsResponse> DnsQueryAsync(
            DnsQuestion dnsQuestion,
            DnsProvider dnsProvider = DnsProvider.Google,
            CancellationToken cancellationToken = default)
        {
            var dnsResponses = await this.QueryDnsQuestions(dnsProvider, [dnsQuestion], cancellationToken);
            return dnsResponses.Single();
        }

        private HttpClient GetHttpClient(DnsProvider dnsProvider)
        {
            HttpClient httpClient;

            if (this._httpClientFactory is not null)
            {
                httpClient = this._httpClientFactory.CreateClient();
            }
            else
            {
                httpClient = this._httpClient!;
            }

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

            await Task.WhenAll(httpQueryTasks).ConfigureAwait(false);

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

            await Task.WhenAll(jsonReadTasks).ConfigureAwait(false);

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
