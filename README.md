# DotNetExtensionKit

[![NuGet](https://img.shields.io/nuget/v/DotNetExtensionKit.svg)](https://www.nuget.org/packages/DotNetExtensionKit)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Docs](https://img.shields.io/badge/docs-online-blue.svg)](https://osamaabusitta.github.io/DotNetExtensionKit/README.html)
[![API Reference](https://img.shields.io/badge/api-reference-green.svg)](https://osamaabusitta.github.io/DotNetExtensionKit/api/DotNetExtensionKit.html)

A lightweight .NET library of extension methods for strings, collections, dates, and general utilities. Targets .NET Standard 2.0 for broad compatibility.

## Features

- **String** — truncation, case conversion, masking, slug generation, Base64, character filtering
- **Collection** — null-safe access, batching, iteration helpers, distinct-by-key
- **DateTime** — day/week/month/year boundaries, age calculation, weekend checks
- **General** — null checks, safe casting, conditional transforms, range and membership checks
- ...

## Installation

```
dotnet add package DotNetExtensionKit
```

Or via the NuGet Package Manager:

```
Install-Package DotNetExtensionKit
```

## Quick Start

```csharp
using DotNetExtensionKit;

// Strings
"Hello, World!".Truncate(5);           // "Hello"
"HelloWorld".ToSnakeCase();            // "hello_world"
"1234567890".Mask();                   // "12******90"
"Hello World!".ToSlug();               // "hello-world"

// Collections
var batches = new[] { 1, 2, 3, 4, 5 }.Batch(2);  // { {1,2}, {3,4}, {5} }
List<int>? items = null;
items.IsNullOrEmpty();                 // true
items.OrEmpty();                       // safe empty enumerable

// DateTime
var now = DateTime.Now;
now.StartOfDay();                      // today at 00:00:00
now.StartOfWeek();                     // most recent Monday
new DateTime(1990, 3, 25).Age();       // age in years

// General
"hello".Transform(s => s.Length);      // 5
5.IsBetween(1, 10);                    // true
"b".IsIn("a", "b", "c");              // true
```

## Documentation

Full API reference is available at the [documentation site](https://osamaabusitta.github.io/DotNetExtensionKit/README.html).

## Compatibility

This library targets [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0#select-net-standard-version), which is supported by:

| Platform | Minimum Version |
|---|---|
| .NET / .NET Core | 2.0+ |
| .NET Framework | 4.6.1+ |
| Mono | 5.4+ |
| Xamarin.iOS | 10.14+ |
| Xamarin.Mac | 3.8+ |
| Xamarin.Android | 8.0+ |
| UWP | 10.0.16299+ |
| Unity | 2018.1+ |

The library uses C# 8.0 with nullable reference types enabled.

## Contributing

Contributions are welcome. Please open an issue to discuss your idea before submitting a pull request.

1. Fork the repository
2. Create a feature branch (`git checkout -b my-feature`)
3. Commit your changes (`git commit -m "Add my feature"`)
4. Push to the branch (`git push origin my-feature`)
5. Open a Pull Request

## License

[MIT](LICENSE)
