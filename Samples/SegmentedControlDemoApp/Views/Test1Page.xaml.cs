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

        private void SegmentedControl_OnSegmentSelected(object sender, Plugin.SegmentedControl.Maui.SegmentSelectEventArgs e)
        {
            this.logger.LogDebug($"SegmentedControl_OnSegmentSelected: {e.NewValue}");
            _ = this.dialogService.DisplayAlertAsync("OnSegmentSelected", $"NewValue={e.NewValue}", "OK");
        }

        private void SegmentedControl_OnElementChildrenChanging(object sender, Plugin.SegmentedControl.Maui.ElementChildrenChanging e)
        {
            this.logger.LogDebug("SegmentedControl_OnElementChildrenChanging");
        }
    }
}