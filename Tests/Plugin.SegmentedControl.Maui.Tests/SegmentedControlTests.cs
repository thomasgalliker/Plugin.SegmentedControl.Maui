using FluentAssertions;
using Xunit;

namespace Plugin.SegmentedControl.Maui.Tests
{
    public class SegmentedControlTests
    {
        [Fact]
        public void ShouldRaiseSegmentSelectedCommand()
        {
            // Arrange
            var segmentSelectedCommand = 0;
            var segmentedControl = CreateSegmentedControl();
            segmentedControl.SegmentSelectedCommand = new Command(() => { segmentSelectedCommand++; });

            // Act
            segmentedControl.RaiseSelectionChanged(1);

            // Assert
            segmentSelectedCommand.Should().Be(1);
        }
        
        [Fact]
        public void ShouldRaiseSegmentSelectedEvent()
        {
            // Arrange
            var segmentSelectedCommand = 0;
            var segmentedControl = CreateSegmentedControl();
            segmentedControl.OnSegmentSelected += (o, e) => { segmentSelectedCommand++; };

            // Act
            segmentedControl.RaiseSelectionChanged(1);

            // Assert
            segmentSelectedCommand.Should().Be(1);
        }

        private static SegmentedControl CreateSegmentedControl()
        {
            // Arrange
            return new SegmentedControl
            {
                Children = new[]
                {
                    new SegmentedControlOption { Text = "Tab 1" },
                    new SegmentedControlOption { Text = "Tab 2" },
                    new SegmentedControlOption { Text = "Tab 3" },
                },
            };
        }
    }
}
