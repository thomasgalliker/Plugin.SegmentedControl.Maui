namespace Plugin.SegmentedControl.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseSegmentedControl(this MauiAppBuilder builder)
        {
#if (ANDROID || IOS)
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(SegmentedControl), typeof(SegmentedControlHandler));
            });
#endif

#if ANDROID
            builder.Services.AddSingleton<ITypefaceResolver, TypefaceResolver>();
#endif

            return builder;
        }
    }
}