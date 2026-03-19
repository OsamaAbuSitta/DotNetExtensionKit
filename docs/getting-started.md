---
uid: getting-started
title: Getting Started
---

# Getting Started

## Installation

Install ExtensionKit from NuGet using the .NET CLI:

```bash
dotnet add package ExtensionKit
```

Or via the NuGet Package Manager Console:

```powershell
Install-Package ExtensionKit
```

## Basic Usage

Add the namespace to your file:

```csharp
using ExtensionKit;
```

### String Extensions

```csharp
// Truncate with ellipsis
"Hello, World!".TruncateWithEllipsis(8); // "Hello..."

// Convert to camelCase
"user full name".ToCamelCase(); // "userFullName"

// Mask sensitive data
"4111111111111111".Mask(4, 4); // "4111********1111"

// Extract digits
"Phone: (555) 123-4567".DigitsOnly(); // "5551234567"
```

### DateTime Extensions

```csharp
var now = DateTime.Now;

// Get day boundaries
var dayStart = now.StartOfDay();  // today at 00:00:00
var dayEnd = now.EndOfDay();      // today at 23:59:59.9999999

// Week and month boundaries
var weekStart = now.StartOfWeek();
var monthEnd = now.EndOfMonth();

// Calculate age
var birthDate = new DateTime(1990, 6, 15);
int age = birthDate.Age(); // age in whole years

// Check weekend
bool isWeekend = now.IsWeekend();
```

### Collection Extensions

```csharp
// Null-safe checks
List<int>? items = null;
items.IsNullOrEmpty(); // true

// Batch a sequence
var batches = Enumerable.Range(1, 10).Batch(3);
// [[1,2,3], [4,5,6], [7,8,9], [10]]

// ForEach with index
new[] { "a", "b", "c" }.ForEach((item, i) =>
    Console.WriteLine($"{i}: {item}"));

// Safe dictionary access
var dict = new Dictionary<string, int> { ["key"] = 42 };
dict.GetValueOrDefault("missing", -1); // -1
```

### General Extensions

```csharp
// Null checks
string? name = null;
name.IsNull();    // true
name.IsNotNull(); // false

// Safe casting
object obj = "hello";
var str = obj.As<string>(); // "hello"

// Conditional return
42.If(true);   // 42
42.If(false);  // 0 (default)

// Transform
"hello".Transform(s => s.Length); // 5

// Range and set checks
5.IsBetween(1, 10); // true
"b".IsIn("a", "b", "c"); // true
```
