using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.SegmentedControl.Maui;
using SegmentedControlDemoApp.Services;
using SegmentedControlDemoApp.Services.Logging;
using SegmentedControlDemoApp.ViewModels;
using SegmentedControlDemoApp.Views;

namespace SegmentedControlDemoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSegmentedControl()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddLogging(b =>
            {
                b.ClearProviders();
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddDebug();
                b.AddSentry(SentryConfiguration.Configure);
            });

            // Register services
            builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<ILauncher>(_ => Launcher.Default);

            // Register pages and view models
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddTransient<Test1Page>();

            builder.Services.AddTransient<Test2Page>();
            builder.Services.AddTransient<Test2ViewModel>();

            builder.Services.AddTransient<Test3Page>();
            builder.Services.AddTransient<Test3ViewModel>();

            builder.Services.AddTransient<Test4Page>();

            builder.Services.AddTransient<Test5Page>();
            builder.Services.AddTransient<Test5ViewModel>();

            builder.Services.AddTransient<MedicationDetailPage>();
            builder.Services.AddTransient<MedicationDetailViewModel>();

            builder.Services.AddTransient<Test5DetailPage>();

            return builder.Build();
        }
    }
}
