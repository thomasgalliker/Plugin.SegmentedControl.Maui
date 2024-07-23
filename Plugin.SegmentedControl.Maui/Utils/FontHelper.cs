using Font = Microsoft.Maui.Font;

namespace Plugin.SegmentedControl.Maui.Utils
{
    internal class FontHelper
    {
        public static Font CreateFont(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            Font font;

            if (fontFamily == null || fontFamily.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
            {
                font = Font.Default;

                if (fontSize > 0d)
                {
                    font = font.WithSize(fontSize);
                }
            }
            else
            {
                font = Font.OfSize(fontFamily, fontSize);
            }

            if (fontAttributes != FontAttributes.None)
            {
                font = font.WithAttributes(fontAttributes);
            }

            return font;
        }
    }
}