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
        public string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Time to live
        /// </summary>
        public int TTL { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; set; }
    }
}
