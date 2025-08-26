# Nager.Dns with DoH (DNS over HTTPS)

**Nager.Dns** is a lightweight and powerful C# DNS client that performs **secure DNS queries over HTTPS (DoH)**. It supports multiple DNS providers and provides an easy-to-use API for .NET applications.

## ⚡ Features
- Secure DNS queries over HTTPS (DoH)
- Bulk DNS query support
- Multiple DNS providers included (e.g., Google, Cloudflare)
- Lightweight and fast
- Easily integrates with .NET applications

## 📦 Installation

The package is available on [NuGet](https://www.nuget.org/packages/Nager.Dns)

```powershell
PM> install-package Nager.Dns
```

## 💻 Usage Examples

### Bulk DNS Query
Perform DNS queries for multiple domains at once:

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsQuestions = new DnsQuestion[]
{
    new DnsQuestion("google.com", DnsRecordType.A),
    new DnsQuestion("microsoft.com", DnsRecordType.A)
};

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.BulkDnsQueryAsync(dnsQuestions, DnsProvider.Google);
```

### Single DNS Query
Perform a DNS query for a single domain:

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.DnsQueryAsync(new DnsQuestion("google.com", DnsRecordType.A), DnsProvider.Google);
```

## 🌐 Supported DNS Providers
- Google Public DNS
- Cloudflare DNS

## 📝 Feedback and Contributions
We welcome feedback, feature requests, and contributions! Feel free to open an issue or submit a pull request.
