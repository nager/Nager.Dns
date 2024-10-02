namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Response Status
    /// </summary>
    public enum DnsResponseStatus
    {
        NoError = 0,
        FormErr = 1,
        ServFail = 2,
        NxDomain = 3,
        NotImp = 4,
        Refused = 5,
        YXDomain = 6,
        YXRRSet = 7,
        NXRRSet = 8,
        NotAuth = 9,
        NotZone = 10
    }
}
