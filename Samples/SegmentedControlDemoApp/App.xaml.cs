using SegmentedControlDemoApp.Views;

namespace SegmentedControlDemoApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            this.InitializeComponent();

            var mainPage = serviceProvider.GetRequiredService<MainPage>();
            // var mainPage = serviceProvider.GetRequiredService<TabbedMainPage>();
            this.MainPage = new NavigationPage(mainPage);
        }
    }
}
