namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Response
    /// </summary>
    public class DnsResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        /// <remarks>DNS response code (32 bit integer)</remarks>
        public int Status { get; set; }

        /// <summary>
        /// DNS response code
        /// </summary>
        /// <remarks>
        /// Parsed <see cref="DnsResponseCode"/> value from the raw <c>Status</c> field,
        /// or <c>null</c> if the status code is not defined in the enum.
        /// </remarks>
        public DnsResponseCode? ResponseCode
        {
            get
            {
                if (Enum.IsDefined(typeof(DnsResponseCode), this.Status))
                {
                    return (DnsResponseCode)this.Status;
                }

                return null;
            }
        }

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
        /// Dns Questions
        /// </summary>
        public DnsQuestion[] Question { get; set; } = [];

        /// <summary>
        /// Answer Dns Records
        /// </summary>
        public DnsRecord[] Answer { get; set; } = [];

        /// <summary>
        /// Authority Dns Records
        /// </summary>
        public DnsRecord[] Authority { get; set; } = [];

        /// <summary>
        /// Additional Dns Records
        /// </summary>
        public DnsRecord[] Additional { get; set; } = [];

        //WARNING: the response sometimes changes
        //public string[] Comment { get; set; }
    }
}
