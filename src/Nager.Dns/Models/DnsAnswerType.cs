namespace Nager.Dns.Models
{
    public enum DnsAnswerType
    {
        A = 1,      // Standard DNS RR type
        NS = 2,     // Name server
        CNAME = 5,  // Canonical name for an alias
        SOA = 6,    // Start of authority for a zone
        PTR = 12,   // Domain name pointer
        MX = 15,    // Mail exchange
        TXT = 16,   // Text strings
        AAAA = 28,  // IPv6 address
        SRV = 33,   // Service locator
        ANY = 255   // Wildcard match
    }
}
