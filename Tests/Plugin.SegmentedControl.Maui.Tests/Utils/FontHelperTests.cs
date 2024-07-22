using FluentAssertions;
using Plugin.SegmentedControl.Maui.Utils;
using Xunit;

namespace Plugin.SegmentedControl.Maui.Tests.Utils
{
    public class FontHelperTests
    {
        [Fact]
        public void ShouldCreateFont()
        {
            // Arrange
            var fontFamily = "monospace";
            var fontSize = 22;
            var fontAttributes = FontAttributes.Bold;

            // Act
            var font = FontHelper.CreateFont(fontFamily, fontSize, fontAttributes);

            // Assert
            font.Should().NotBeNull();
            font.Size.Should().Be(fontSize);
            font.Family.Should().Be(fontFamily);
        }

        // TODO: Add more unit tests here!
    }
}
