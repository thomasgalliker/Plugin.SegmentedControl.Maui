SegmentSelectEventArgs# Plugin.SegmentedControl.Maui
[![Version](https://img.shields.io/nuget/v/Plugin.SegmentedControl.Maui.svg)](https://www.nuget.org/packages/Plugin.SegmentedControl.Maui)  [![Downloads](https://img.shields.io/nuget/dt/Plugin.SegmentedControl.Maui.svg)](https://www.nuget.org/packages/Plugin.SegmentedControl.Maui)

This library provides a segmented control for .NET MAUI apps using native platform APIs.


### Download and Install Plugin.SegmentedControl.Maui
This library is available on NuGet: https://www.nuget.org/packages/Plugin.SegmentedControl.Maui
Use the following command to install Plugin.FirebasePushNotifications using NuGet package manager console:

    PM> Install-Package Plugin.SegmentedControl.Maui

You can use this library in any .NET MAUI project compatible to .NET 8 and higher.

#### App Setup
This plugin provides an extension method for MauiAppBuilder `UseSegmentedControl` which ensure proper startup and initialization.
Call this method within your `MauiProgram` just as demonstrated in the [SegmentedControlDemoApp](https://github.com/thomasgalliker/Plugin.SegmentedControl.Maui/tree/develop/Samples):
```csharp
var builder = MauiApp.CreateBuilder()
    .UseMauiApp<App>()
    .UseSegmentedControl();
```

### API Usage
`tbd`

### Contribution
Contributors welcome! If you find a bug or you want to propose a new feature, feel free to do so by opening a new issue on github.com.

### Links
- https://developer.apple.com/documentation/uikit/uisegmentedcontrol
- https://developer.android.com/reference/android/widget/RadioGroup