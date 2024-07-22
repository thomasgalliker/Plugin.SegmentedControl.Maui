using Android.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Font = Microsoft.Maui.Graphics.Font;

namespace Plugin.SegmentedControl.Maui
{
    public static class FontManagerPlatform
    {
        #region Declarations

        private static List<TypeFaceHolder> Typefaces { get; } = new();

        #endregion

        public static void Dispose()
        {
            foreach (var typeface in Typefaces)
            {
                typeface.Typeface.Dispose();
            }

            Typefaces.Clear();
        }

        public static Typeface GetTypeFace(string fontFamily)
        {
            if (string.IsNullOrWhiteSpace(fontFamily) || fontFamily.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                return Typeface.Default;
            }

            foreach (var typeface in
                     from TypeFaceHolder typeface in Typefaces
                     where typeface.FontFamily == fontFamily
                     select typeface)
            {
                return typeface.Typeface;
            }

            var typeFaceHolder = new TypeFaceHolder
            {
                FontFamily = fontFamily,
                Typeface = new Font(fontFamily).ToTypeface()
            };

            Typefaces.Add(typeFaceHolder);
            return typeFaceHolder.Typeface;
        }

        private class TypeFaceHolder
        {
            public string FontFamily { get; set; }
            public Typeface Typeface { get; set; }
        }
    }
}