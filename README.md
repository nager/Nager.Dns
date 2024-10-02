# Nager.Dns with DoH (DNS over HTTPS)

A simple C# DNS client that uses DoH to securely perform DNS queries over HTTPS.

```cs
var dnsClient = new DnsClient(new HttpClient());
var responses = await dnsClient.MultiQueryAsync(DnsProvider.Google, [new DnsQuestion("google.com", DnsAnswerType.A)]);
```
