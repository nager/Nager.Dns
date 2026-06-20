using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Question
    /// </summary>
    public class DnsQuestion
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public DnsRecordType Type { get; set; }

        /// <summary>
        /// Dns Question
        /// </summary>
        public DnsQuestion() { }

        /// <summary>
        /// Dns Question
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        [SetsRequiredMembers]
        public DnsQuestion(string name, DnsRecordType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
