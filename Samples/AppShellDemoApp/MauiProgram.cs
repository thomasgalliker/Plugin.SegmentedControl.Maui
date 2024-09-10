using Microsoft.Extensions.Logging;
using Plugin.SegmentedControl.Maui;
using SegmentedControlReproduce.ViewModels;
using SegmentedControlReproduce.Views;

namespace SegmentedControlReproduce
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSegmentedControl()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Logging.AddDebug();
            builder.Services.AddSingleton<VowelsViewModel>();
            builder.Services.AddTransient<VowelsView>();

            return builder.Build();
        }
    }
}
