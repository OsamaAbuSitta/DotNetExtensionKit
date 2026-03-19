# DotNetExtensionKit

A lightweight .NET library of extension methods for strings, collections, dates, and general utilities. Targets .NET Standard 2.0 for broad compatibility.

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

## API Overview

### StringExtensions

| Method | Description |
|---|---|
| `IsNullOrEmpty` | Check for null or empty |
| `IsNullOrWhiteSpace` | Check for null, empty, or whitespace |
| `HasValue` | Inverse of IsNullOrWhiteSpace |
| `NullIfEmpty` / `NullIfWhiteSpace` | Return null instead of empty/whitespace |
| `EmptyIfNull` | Return empty string instead of null |
| `Truncate` / `TruncateWithEllipsis` / `TruncateWords` | Shorten strings |
| `Left` / `Right` | Substring from start or end |
| `SplitByCasing` | Insert spaces in PascalCase/camelCase |
| `ToTitleCaseInvariant` / `ToCamelCase` / `ToSnakeCase` / `ToKebabCase` | Case conversions |
| `Mask` | Mask middle characters |
| `DigitsOnly` / `LettersOnly` / `AlphaNumericOnly` | Filter characters |
| `RemoveWhitespace` / `CollapseWhitespace` | Whitespace manipulation |
| `SplitLines` | Split by line breaks |
| `ToBase64` / `FromBase64` | Base64 encoding/decoding |
| `ToSlug` | URL-friendly slug |

### DateTimeExtensions

| Method | Description |
|---|---|
| `StartOfDay` / `EndOfDay` | Day boundaries |
| `StartOfWeek` / `EndOfWeek` | Week boundaries (configurable start day) |
| `StartOfMonth` / `EndOfMonth` | Month boundaries |
| `StartOfYear` / `EndOfYear` | Year boundaries |
| `Age` | Calculate age in years |
| `IsWeekend` / `IsWeekday` | Day-of-week checks |
| `IsBetween` | Range check |

### CollectionExtensions

| Method | Description |
|---|---|
| `IsNullOrEmpty` / `HasItems` | Null/empty checks |
| `OrEmpty` / `OrEmptyList` | Null-safe enumeration |
| `Batch` | Split into fixed-size batches |
| `ForEach` | Iterate with action (with optional index) |
| `DistinctBy` | Distinct by key selector |
| `FirstOrDefault` | First element with custom default |
| `GetValueOrDefault` | Dictionary lookup with default |

### GeneralExtensions

| Method | Description |
|---|---|
| `IsNull` / `IsNotNull` | Null checks |
| `As` | Safe cast |
| `If` / `IfNot` | Conditional value return |
| `Transform` | Pipe a value through a function |
| `Also` | Execute side-effect, return original value |
| `IsBetween` | Inclusive range check |
| `IsIn` | Check membership in a set |
| `CallGenericMethod` | Invoke generic methods via reflection |

## Compatibility

- .NET Standard 2.0 (.NET Framework 4.6.1+, .NET Core 2.0+, .NET 5+)
- C# 8.0 nullable reference types enabled

## License

[MIT](LICENSE)
