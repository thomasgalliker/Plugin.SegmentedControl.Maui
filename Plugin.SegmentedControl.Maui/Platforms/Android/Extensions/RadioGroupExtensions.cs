using Android.Widget;
using RadioButton = Android.Widget.RadioButton;

namespace Plugin.SegmentedControl.Maui.Platforms.Extensions
{
    internal static class RadioGroupExtensions
    {
        internal static RadioButton GetRadioButtonAt(this RadioGroup radioGroup, int index)
        {
            var radioButton = radioGroup.GetChildAt(index) as RadioButton;
            return radioButton;
        }

        internal static IEnumerable<RadioButton> GetRadioButtons(this RadioGroup radioGroup)
        {
            var childCount = radioGroup.ChildCount;
            if (childCount > 0)
            {
                for (var i = 0; i < childCount; i++)
                {
                    if (radioGroup.GetChildAt(i) is RadioButton radioButton)
                    {
                        yield return radioButton;
                    }
                }
            }
        }
    }
}
