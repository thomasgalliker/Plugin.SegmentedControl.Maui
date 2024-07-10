using Microsoft.Extensions.Logging;
using Sentry.Extensions.Logging;

namespace SegmentedControlDemoApp.Services.Logging
{
    public static class SentryConfiguration
    {
        public static void Configure(SentryLoggingOptions options)
        {
            options.InitializeSdk = true;
#if DEBUG
            options.Debug = true;
#endif
            options.Dsn = "https://81933794fef907d63e866ccab72248f1@o4507458300280832.ingest.de.sentry.io/4507570985173072";
            options.MinimumEventLevel = LogLevel.Warning;
            options.MinimumBreadcrumbLevel = LogLevel.Debug;
        }
    }
}