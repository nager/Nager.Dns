# Nager.Dns with DoH (DNS over HTTPS)

Nager.Dns is a simple and powerful C# DNS client that securely performs DNS queries using DNS over HTTPS (DoH). It supports multiple DNS providers and offers an easy-to-use API.

## Features
- Secure DNS queries over HTTPS (DoH)
- Bulk DNS query support
- Multiple DNS providers included (e.g., Google, Cloudflare)
- Lightweight and fast
- Easily integrates with .NET applications

## Installation

The package is available on [nuget](https://www.nuget.org/packages/Nager.Dns)
```
PM> install-package Nager.Dns
```

## Usage

### Bulk DNS Query Example
Perform DNS queries for multiple domains at once:

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsQuestions = new DnsQuestion[]
{
    new DnsQuestion("google.com", DnsAnswerType.A),
    new DnsQuestion("microsoft.com", DnsAnswerType.A)
};

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.BulkDnsQueryAsync(dnsQuestions, DnsProvider.Google);
```

### Single DNS Query Example
Perform a DNS query for a single domain:

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.DnsQueryAsync(new DnsQuestion("google.com", DnsAnswerType.A), DnsProvider.Google);
```

## Supported DNS Providers
- Google Public DNS
- Cloudflare DNS

## Feedback and Contributions
We welcome feedback, feature requests, and contributions! Feel free to open an issue or submit a pull request.
