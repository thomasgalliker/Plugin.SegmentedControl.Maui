using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.Views
{
    public partial class Test1Page : ContentPage
    {
        private readonly ILogger logger;
        private readonly IDialogService dialogService;

        public Test1Page(ILogger<Test1Page> logger, IDialogService dialogService)
        {
            this.InitializeComponent();

            this.logger = logger;
            this.dialogService = dialogService;
        }

        private void SegmentedControl_SelectedIndexChanged(object sender, Plugin.SegmentedControl.Maui.SelectedIndexChangedEventArgs e)
        {
            this.logger.LogDebug($"SegmentedControl_SelectedIndexChanged: {e.NewValue}");
            _ = this.dialogService.DisplayAlertAsync("SelectedIndexChanged", $"NewValue={e.NewValue}", "OK");
        }

        private void SegmentedControl_ChildrenChanging(object sender, Plugin.SegmentedControl.Maui.ChildrenChangingEventArgs e)
        {
            this.logger.LogDebug("SegmentedControl_ChildrenChanging");
        }
    }
}