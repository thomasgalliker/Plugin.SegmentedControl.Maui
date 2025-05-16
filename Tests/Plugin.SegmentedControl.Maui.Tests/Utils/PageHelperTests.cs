using FluentAssertions;
using Plugin.SegmentedControl.Maui.Utils;
using Xunit;

namespace Plugin.SegmentedControl.Maui.Tests.Utils
{
    public class PageHelperTests
    {
        public PageHelperTests()
        {
            MauiMocks.Init();
        }

        [Fact]
        public void ShouldGetNavigationTree_NavigationPageWithTabbedPage()
        {
            // Arrange
            var page = new NavigationPage(new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ContentPage()),
                    new ContentPage()
                }
            });

            // Act
            var pages = PageHelper.GetNavigationTree(page).ToArray();

            // Assert
            pages.Should().HaveCount(5);
        }
    }
}