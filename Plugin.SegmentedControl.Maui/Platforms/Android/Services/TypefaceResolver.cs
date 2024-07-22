using Android.Graphics;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics.Platform;
using Font = Microsoft.Maui.Font;

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
                t.FontAttrbutes == t.FontAttrbutes);

            if (typefaceCache != null)
            {
                // Use cached value
            }
            else
            {
                try
                {
                    Font font;

                    if (fontFamily != null && !fontFamily.Equals("Default", StringComparison.InvariantCultureIgnoreCase) && fontSize > 0d)
                    {
                        font = Font.OfSize(fontFamily, fontSize);
                    }
                    else
                    {
                        font = Font.Default;

                        if (fontSize > 0d)
                        {
                            font = font.WithSize(fontSize);
                        }
                    }

                    if (fontAttributes != FontAttributes.None)
                    {
                        font = font.WithAttributes(fontAttributes);
                    }

                    var typeface = font.ToTypeface(this.fontManager);

                    typefaceCache = new TypefaceCache
                    {
                        FontFamily = fontFamily,
                        FontSize = fontSize,
                        FontAttrbutes = fontAttributes,
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

            public FontAttributes FontAttrbutes { get; set; }

            public Typeface Typeface { get; set; }
        }

        public void Dispose()
        {
            foreach (var typeface in this.typefaceCaches)
            {
                typeface.Typeface.Dispose();
            }

            this.typefaceCaches.Clear();
        }
    }
}