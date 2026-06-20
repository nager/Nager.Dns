using Nager.Dns.Models;
using System.Text.Json.Serialization;

namespace Nager.Dns
{
    [JsonSerializable(typeof(DnsResponse))]
    internal partial class DnsResponseJsonContext : JsonSerializerContext
    {
    }
}
