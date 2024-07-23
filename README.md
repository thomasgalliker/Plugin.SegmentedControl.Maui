
# SegmentedControl for .NET MAUI

[![Version](https://img.shields.io/nuget/v/Plugin.SegmentedControl.Maui.svg)](https://www.nuget.org/packages/Plugin.SegmentedControl.Maui)  [![Downloads](https://img.shields.io/nuget/dt/Plugin.SegmentedControl.Maui.svg)](https://www.nuget.org/packages/Plugin.SegmentedControl.Maui)

This library provides a segmented control for .NET MAUI apps using native platform APIs.

### Download and Install Plugin.SegmentedControl.Maui
This library is available on NuGet: [Plugin.SegmentedControl.Maui](https://www.nuget.org/packages/Plugin.SegmentedControl.Maui)  
Use the following command to install `Plugin.SegmentedControl.Maui` using the NuGet package manager console:

```powershell
PM> Install-Package Plugin.SegmentedControl.Maui
```

You can use this library in any .NET MAUI project compatible with .NET 8 and higher.

#### App Setup
This plugin provides an extension method for `MauiAppBuilder` called `UseSegmentedControl`, which ensures proper startup and initialization.  
Call this method within your `MauiProgram` just as demonstrated in the [SegmentedControlDemoApp](https://github.com/thomasgalliker/Plugin.SegmentedControl.Maui/tree/develop/Samples):

```csharp
var builder = MauiApp.CreateBuilder()
    .UseMauiApp<App>()
    .UseSegmentedControl();
```

### API Usage
The `SegmentedControl` class provides several properties to customize its behavior and appearance. Below is a table detailing each property and its purpose.

| Properties                       | Description                                                                                         |
|----------------------------------|-----------------------------------------------------------------------------------------------------|
| `AutoDisconnectHandler`          | Indicates whether the platform handler disconnects automatically. Default is `true`.               |
| `Children`                       | A list of `SegmentedControlOption` objects representing each segment.                               |
| `ItemsSource`                    | The source of data for generating segments.                                                         |
| `TextPropertyName`               | The name of the property to use for the segment's text when using a non-string `ItemsSource`.       |
| `TextColor`                      | The color of the segment text. Default is `Colors.Black`.                                           |
| `TintColor`                      | The color used to tint the control. Default is `Colors.Blue`.                                       |
| `SelectedTextColor`              | The color of the selected segment's text. Default is `Colors.White`.                                |
| `DisabledBackgroundColor`        | The background color of the control when disabled. Default is `Colors.Gray`.                        |
| `DisabledTintColor`              | The tint color of the control when disabled. Default is `Colors.Gray`.                              |
| `DisabledTextColor`              | The text color of the segments when the control is disabled. Default is `Colors.Gray`.              |
| `DisabledSelectedTextColor`      | The text color of the selected segment when the control is disabled. Default is `Colors.LightGray`. |
| `BorderColor`                    | The color of the control's border. Defaults to the value of `TintColor`.                            |
| `BorderWidth`                    | The width of the control's border. Varies by platform; `1.0` on Android and `0.0` on others.        |
| `SelectedSegment`                | The index of the currently selected segment. Default is `0`.                                        |
| `SelectedItem`                   | The currently selected item from the `ItemsSource`.                                                 |
| `SegmentSelectedCommand`         | Command that is executed when a segment is selected.                                                |
| `SegmentSelectedCommandParameter`| Parameter passed to the `SegmentSelectedCommand`.                                                   |
| `FontSize`                       | The size of the font used for the segment text.                                                     |
| `FontFamily`                     | The font family used for the segment text.                                                          |
| `FontAttributes`                 | Font attributes for the segment text, such as bold or italic.                                       |

### Contribution
Contributors welcome! If you find a bug or want to propose a new feature, feel free to do so by opening a new issue on GitHub.

### Links
- https://developer.apple.com/documentation/uikit/uisegmentedcontrol
- https://developer.android.com/reference/android/widget/RadioGroup