namespace Nager.Dns.Models
{
    /// <summary>
    /// DnsResponse
    /// </summary>
    public class DnsResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Truncated bit was set
        /// </summary>
        /// <remarks>This happens when the DNS answer is larger than a single UDP or TCP packet</remarks>
        public bool TC { get; set; }

        /// <summary>
        /// Recursion Desired
        /// </summary>
        public bool RD { get; set; }

        /// <summary>
        /// Recursion Available
        /// </summary>
        public bool RA { get; set; }

        /// <summary>
        /// Authentic Data
        /// </summary>
        public bool AD { get; set; }

        /// <summary>
        /// Checking Disabled (DNSSEC)
        /// </summary>
        public bool CD { get; set; }

        /// <summary>
        /// Question
        /// </summary>
        public DnsQuestion[] Question { get; set; }

        public DnsRecord[] Answer { get; set; }

        public DnsRecord[] Authority { get; set; }

        public DnsRecord[] Additional { get; set; }

        //WARNING: the response sometimes changes
        //public string[] Comment { get; set; }
    }
}
