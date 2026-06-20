using System.Text.Json.Serialization;

namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Record
    /// </summary>
    public class DnsRecord
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Type Id
        /// </summary>
        [JsonPropertyName("type")]
        public int TypeId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        /// <remarks>
        /// Parsed <see cref="DnsRecordType"/> value from the raw <c>TypeId</c> field,
        /// or <c>null</c> if the record type is not defined in the enum.
        /// </remarks>
        [JsonIgnore]
        public DnsRecordType? Type
        {
            get
            {
                if (Enum.IsDefined(typeof(DnsRecordType), this.TypeId))
                {
                    return (DnsRecordType)this.TypeId;
                }

                return null;
            }
        }

        /// <summary>
        /// Time to live
        /// </summary>
        [JsonPropertyName("TTL")]
        public int TTL { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }
}
