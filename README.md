# CefGlue

This is an attempt to make CefGlue available for Linux systems. Currently work in progress.

.NET binding for The Chromium Embedded Framework (CEF). 

CefGlue lets you embed Chromium in .NET apps. It is a .NET wrapper control around the Chromium Embedded Framework ([CEF](https://bitbucket.org/chromiumembedded/cef/src/master/)). 
It can be used from C# or any other CLR language and provides both Avalonia and WPF web browser control implementations.
The Avalonia implementation runs on Windows and macOS. Linux is partially supported.

Currently only x64 and ARM64 architectures are supported.

## Releases
Stable binaries are released on NuGet, and contain everything you need to embed Chromium in your .NET/CLR application. 
- [![CefGlue.Avalonia](https://img.shields.io/nuget/v/CefGlue.Avalonia.svg?style=flat&label=CefGlue-Avalonia)](https://www.nuget.org/packages/CefGlue.Avalonia/)
- [![CefGlue.Avalonia.ARM64](https://img.shields.io/nuget/v/CefGlue.Avalonia.ARM64.svg?style=flat&label=CefGlue-Avalonia-ARM64)](https://www.nuget.org/packages/CefGlue.Avalonia.ARM64/)
- [![CefGlue.Common](https://img.shields.io/nuget/v/CefGlue.Common.svg?style=flat&label=CefGlue-Common)](https://www.nuget.org/packages/CefGlue.Common/)
- [![CefGlue.Common.ARM64](https://img.shields.io/nuget/v/CefGlue.Common.ARM64.svg?style=flat&label=CefGlue-Common-ARM64)](https://www.nuget.org/packages/CefGlue.Common.ARM64/)
- [![CefGlue.WPF](https://img.shields.io/nuget/v/CefGlue.WPF.svg?style=flat&label=CefGlue-WPF)](https://www.nuget.org/packages/CefGlue.WPF/)
- [![CefGlue.WPF.ARM64](https://img.shields.io/nuget/v/CefGlue.WPF.ARM64.svg?style=flat&label=CefGlue-WPF-ARM64)](https://www.nuget.org/packages/CefGlue.WPF.ARM64/)

## Documentation 
See the [Avalonia sample](CefGlue.Demo.Avalonia) or [WPF sample](CefGlue.Demo.WPF) projects for example web browsers built with CefGlue. They demo some of the available features.

## Contrubution

**Temporary** building and debugging instructions.

### Build and debug linux-x64 on Windows

Build:

This build steps are a little complicated but I'll try my best to make them automated in the future.

```powershell
# Build linux-x64 on Windows

# 1. Restore the packages
dotnet restore -p:Platform=x64

# 2. Build the BrowserProcess (MUSTBE net6.0)
dotnet build -r linux-x64 -f net6.0 .\CefGlue.BrowserProcess

# 3. Build the demo
dotnet build --runtime linux-x64 -f net6.0 -c Debug_WindowlessRender --self-contained .\CefGlue.Demo.Avalonia

# 4. Copy the CEF distribution from packages\cef.bin.linux64\120.1.8\CEF to the demo output
# 5. Copy the BrowserProcess from CefGlue.BrowserProcess\bin\Debug\net6.0\linux-x64\publish to the demo output (MUST OVERRIDE the demo output)
```

Debug:

Run the demo with the following command:

```bash
./Xilium.CefGlue.Demo.Avalonia --wait-for-attach
```

Then attach the debugger to the process.
