namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Answer Type
    /// </summary>
    public enum DnsAnswerType
    {
        /// <summary>
        /// Address record
        /// </summary>
        A = 1,

        /// <summary>
        /// Name server record
        /// </summary>
        NS = 2,

        /// <summary>
        /// Canonical name record
        /// </summary>
        CNAME = 5,

        /// <summary>
        /// Start of authority record
        /// </summary>
        SOA = 6,

        /// <summary>
        /// PTR Resource Record
        /// </summary>
        PTR = 12,

        /// <summary>
        /// Mail exchange record
        /// </summary>
        MX = 15,

        /// <summary>
        /// 	Text record
        /// </summary>
        TXT = 16,

        /// <summary>
        /// IPv6 address record
        /// </summary>
        AAAA = 28,

        /// <summary>
        /// Service locator
        /// </summary>
        SRV = 33,

        /// <summary>
        /// All cached records
        /// </summary>
        ANY = 255
    }
}
