namespace Plugin.SegmentedControl.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseSegmentedControl(this MauiAppBuilder builder)
        {
#if (ANDROID || IOS)
            builder.ConfigureMauiHandlers(handlers =>
            {
                //handlers.AddHandler<..., ...Handler>();
            });
#endif

            return builder;
        }
    }
}