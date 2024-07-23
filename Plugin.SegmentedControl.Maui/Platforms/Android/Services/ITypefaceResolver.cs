using Android.Graphics;

namespace Plugin.SegmentedControl.Maui
{
    public interface ITypefaceResolver
    {
        Typeface GetTypeface(string fontFamily, double fontSize, FontAttributes fontAttrbutes);
    }
}