using FluentAssertions;
using Xunit;

namespace Plugin.SegmentedControl.Maui.Tests
{
    public class SegmentedControlOptionTests
    {
        [Fact]
        public void ShouldCreateSegmentedControlOption_WithItemAndTextPropertyName()
        {
            // Arrange
            var testObject = new TestObject(name: "Test 1", value: 1);
            var segmentedControlOption = new SegmentedControlOption
            {
                Item = testObject,
                TextPropertyName = "Name",
            };

            // Act
            testObject.Name = "Test 2";

            // Assert
            segmentedControlOption.Text.Should().Be("Test 2");
        }

        internal class TestObject : BindableObject
        {
            private string name;

            public TestObject(string name, int value)
            {
                this.Name = name;
                this.Value = value;
            }

            public string Name
            {
                get => this.name;
                set
                {
                    this.name = value;
                    this.OnPropertyChanged(nameof(this.Name));
                }
            }

            public int Value { get; set; }
        }
    }
}
