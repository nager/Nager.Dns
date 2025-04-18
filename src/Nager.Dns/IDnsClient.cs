using Nager.Dns.Models;
using System.Collections.ObjectModel;

namespace Nager.Dns
{
    /// <summary>
    /// Dns Client Interface
    /// </summary>
    public interface IDnsClient
    {
        /// <summary>
        /// Performs a bulk DNS query for multiple questions, with optional concurrency control.
        /// </summary>
        /// <param name="dnsQuestions">The list of DNS questions to resolve.</param>
        /// <param name="dnsProvider">The DNS provider to use. Default to <see cref="DnsProvider.Google"/></param>
        /// <param name="maxConcurrentRequests">The maximum number of concurrent requests. Defaults to 20.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns></returns>
        Task<ReadOnlyCollection<DnsResponse>> BulkDnsQueryAsync(
            IEnumerable<DnsQuestion> dnsQuestions,
            DnsProvider dnsProvider = DnsProvider.Google,
            int maxConcurrentRequests = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs a DNS query for a single question.
        /// </summary>
        /// <param name="dnsQuestion">The DNS question to resolve.</param>
        /// <param name="dnsProvider">The DNS provider to use. Default to <see cref="DnsProvider.Google"/></param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns></returns>
        Task<DnsResponse> DnsQueryAsync(
            DnsQuestion dnsQuestion,
            DnsProvider dnsProvider = DnsProvider.Google,
            CancellationToken cancellationToken = default);
    }
}