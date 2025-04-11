namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Response Code
    /// </summary>
    public enum DnsResponseCode
    {
        /// <summary>
        /// DNS Query completed successfully
        /// </summary>
        NoError = 0,

        /// <summary>
        /// DNS Query Format Error
        /// </summary>
        FormErr = 1,

        /// <summary>
        /// Server failed to complete the DNS request
        /// </summary>
        ServFail = 2,

        /// <summary>
        /// Domain name does not exist
        /// </summary>
        NxDomain = 3,

        /// <summary>
        /// Function not implemented
        /// </summary>
        NotImp = 4,

        /// <summary>
        /// The server refused to answer for the query
        /// </summary>
        Refused = 5,

        /// <summary>
        /// Name that should not exist, does exist
        /// </summary>
        YXDomain = 6,

        /// <summary>
        /// RRset that should not exist, does exist
        /// </summary>
        YXRRSet = 7,

        /// <summary>
        /// Server not authoritative for the zone
        /// </summary>
        NotAuth = 8,

        /// <summary>
        /// Name not in zone
        /// </summary>
        NotZone = 9
    }
}
