using Plugin.SegmentedControl.Maui.Utils;

namespace SegmentedControlDemoApp.Views
{
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage(IServiceProvider serviceProvider)
        {
            this.InitializeComponent();

            var testpage1 = serviceProvider.GetRequiredService<Test1Page>();
            var testpage2 = serviceProvider.GetRequiredService<Test2Page>();
            var testpage3 = serviceProvider.GetRequiredService<Test3Page>();
            var testpage4 = serviceProvider.GetRequiredService<Test4Page>();
            var testpage5 = serviceProvider.GetRequiredService<Test5Page>();

            this.Children.Add(testpage1);
            this.Children.Add(testpage2);
            this.Children.Add(testpage3);
            this.Children.Add(testpage4);
            this.Children.Add(testpage5);
        }
    }
}