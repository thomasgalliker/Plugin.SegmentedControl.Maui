using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.Views
{
    public partial class Test5DetailPage : ContentPage
    {
        private readonly INavigationService navigationService;

        public Test5DetailPage(INavigationService navigationService)
        {
            this.InitializeComponent();

            this.navigationService = navigationService;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            _ = this.navigationService.PopModalAsync();
        }
    }
}