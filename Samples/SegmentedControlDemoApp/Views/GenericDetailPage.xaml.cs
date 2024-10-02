using System.Diagnostics;
using Plugin.SegmentedControl.Maui.Utils;
using SegmentedControlDemoApp.Utils;

namespace SegmentedControlDemoApp.Views
{
    public partial class GenericDetailPage : ContentPage, IDebugPage
    {
        private readonly string debugId = IdGenerator.GetNextId();

        public GenericDetailPage()
        {
            this.InitializeComponent();
            this.Title = $"GenericDetailPage {this.debugId}";
        }

        public string DebugId => this.debugId;

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            this.LogNavigation(nameof(this.OnNavigatedTo));
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            this.LogNavigation(nameof(this.OnNavigatedFrom));
        }

        private void LogNavigation(string method)
        {
            var navigationPath = PageHelper.PrintNavigationPath();
            Debug.WriteLine($"{nameof(GenericDetailPage)}[{this.DebugId}].{method}{Environment.NewLine}" +
                            $"> navigationPath: {navigationPath}");
        }

        public override string ToString()
        {
            return $"GenericDetailPage {this.DebugId}";
        }
    }
}