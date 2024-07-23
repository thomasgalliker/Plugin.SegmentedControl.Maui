using FluentAssertions;
using Plugin.SegmentedControl.Maui.Utils;
using Xunit;

namespace Plugin.SegmentedControl.Maui.Tests.Utils
{
    public class FontHelperTests
    {
        [Theory]
        [ClassData(typeof(FontTestData))]
        public void ShouldCreateFont(string fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            // Act
            var font = FontHelper.CreateFont(fontFamily, fontSize, fontAttributes);

            // Assert
            font.Should().NotBeNull();
            font.Size.Should().Be(fontSize);
            font.Family.Should().Be(fontFamily);
        }

        internal class FontTestData : TheoryData<string, double, FontAttributes>
        {
            public FontTestData()
            {
                // FontSize only
                this.Add(null, 11d, FontAttributes.None);

                // FontFamily only
                this.Add("serif", 0d, FontAttributes.None);

                // FontAttributes only
                this.Add(null, 0d, FontAttributes.Bold | FontAttributes.Italic);

                // All font properties set
                this.Add("monospace", 22d, FontAttributes.Bold);
            }
        }
    }
}
