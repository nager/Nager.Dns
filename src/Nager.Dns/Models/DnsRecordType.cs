namespace Nager.Dns.Models
{
    /// <summary>
    /// Dns Record Type
    /// </summary>
    public enum DnsRecordType
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
        /// Host Information
        /// </summary>
        HINFO = 13,

        /// <summary>
        /// Mail exchange record
        /// </summary>
        MX = 15,

        /// <summary>
        /// Text record
        /// </summary>
        TXT = 16,

        /// <summary>
        /// Responsible Person
        /// </summary>
        RP = 17,

        /// <summary>
        /// AFS database record
        /// </summary>
        AFSDB = 18,

        /// <summary>
        /// Signature
        /// </summary>
        SIG = 24,

        /// <summary>
        /// Key record
        /// </summary>
        KEY = 25,

        /// <summary>
        /// IPv6 address record
        /// </summary>
        AAAA = 28,

        /// <summary>
        /// Location record
        /// </summary>
        LOC = 29,

        /// <summary>
        /// Service locator
        /// </summary>
        SRV = 33,

        /// <summary>
        /// Delegation name record
        /// </summary>
        DNAME = 39,

        /// <summary>
        /// Delegation signer
        /// </summary>
        DS = 43,

        /// <summary>
        /// SSH Public Key Fingerprint
        /// </summary>
        SSHFP = 44,

        /// <summary>
        /// DNSSEC signature
        /// </summary>
        RRSIG = 46,

        /// <summary>
        /// Next Secure record
        /// </summary>
        NSEC = 47,

        /// <summary>
        /// DNS Key record
        /// </summary>
        DNSKEY = 48,

        /// <summary>
        /// Next Secure record version 3
        /// </summary>
        NSEC3 = 50,

        /// <summary>
        /// TLSA certificate association
        /// </summary>
        TLSA = 52,

        /// <summary>
        /// OpenPGP public key record
        /// </summary>
        OPENPGPKEY = 61,

        /// <summary>
        /// Service Binding
        /// </summary>
        SVCB = 64,

        /// <summary>
        /// Transaction Key record
        /// </summary>
        TKEY = 249,

        /// <summary>
        /// Transaction Signature
        /// </summary>
        TSIG = 250,

        /// <summary>
        /// All cached records
        /// </summary>
        ANY = 255,

        /// <summary>
        /// Uniform Resource Identifier
        /// </summary>
        URI = 256,

        /// <summary>
        /// Certification Authority Authorization
        /// </summary>
        CAA = 257
    }
}
