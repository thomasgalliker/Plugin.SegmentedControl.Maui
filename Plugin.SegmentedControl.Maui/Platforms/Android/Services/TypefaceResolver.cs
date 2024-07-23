using Android.Graphics;
using Microsoft.Extensions.Logging;
using Plugin.SegmentedControl.Maui.Utils;

namespace Plugin.SegmentedControl.Maui
{
    public class TypefaceResolver : ITypefaceResolver, IDisposable
    {
        private readonly List<TypefaceCache> typefaceCaches = new();
        private readonly ILogger logger;
        private readonly IFontManager fontManager;

        public TypefaceResolver(
            ILogger<TypefaceResolver> logger,
            IFontManager fontManager)
        {
            this.logger = logger;
            this.fontManager = fontManager;
        }

        public Typeface GetTypeface(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            if ((fontFamily == null || fontFamily.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                && fontSize <= 0d &&
                fontAttributes == FontAttributes.None)
            {
                return Typeface.Default;
            }

            var typefaceCache = this.typefaceCaches.SingleOrDefault(t =>
                string.Equals(t.FontFamily, fontFamily, StringComparison.InvariantCultureIgnoreCase) &&
                t.FontSize == fontSize &&
                t.FontAttributes == fontAttributes);

            if (typefaceCache != null)
            {
                // Use cached value
            }
            else
            {
                try
                {
                    var font = FontHelper.CreateFont(fontFamily, fontSize, fontAttributes);
                    var typeface = this.fontManager.GetTypeface(font);

                    typefaceCache = new TypefaceCache
                    {
                        FontFamily = fontFamily,
                        FontSize = fontSize,
                        FontAttributes = fontAttributes,
                        Typeface = typeface
                    };

                    this.typefaceCaches.Add(typefaceCache);

                    return typefaceCache.Typeface;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "GetTypeface failed with exception");
                    return Typeface.Default;
                }
            }

            return typefaceCache.Typeface;
        }

        private class TypefaceCache
        {
            public string FontFamily { get; set; }

            public double FontSize { get; set; }

            public FontAttributes FontAttributes { get; set; }

            public Typeface Typeface { get; set; }
        }

        public void Dispose()
        {
            foreach (var typefaceCache in this.typefaceCaches)
            {
                typefaceCache.Typeface.Dispose();
            }

            this.typefaceCaches.Clear();
        }
    }
}