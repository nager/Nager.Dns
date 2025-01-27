# Nager.Dns with DoH (DNS over HTTPS)

A simple C# DNS client that uses DoH to securely perform DNS queries over HTTPS.

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.BulkDnsQueryAsync([new DnsQuestion("google.com", DnsAnswerType.A), new DnsQuestion("microsoft.com", DnsAnswerType.A)], DnsProvider.Google);
```

```cs
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

var dnsClient = new DnsClient(httpClientFactory);
var responses = await dnsClient.DnsQueryAsync(new DnsQuestion("google.com", DnsAnswerType.A), DnsProvider.Google);
```

## nuget

The package is available on [nuget](https://www.nuget.org/packages/Nager.Dns)
```
PM> install-package Nager.Dns
```
