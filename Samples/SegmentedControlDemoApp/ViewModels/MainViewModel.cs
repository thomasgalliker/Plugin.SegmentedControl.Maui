using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ILogger logger;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        private IAsyncRelayCommand appearingCommand;
        private bool isInitialized;
        private IAsyncRelayCommand<string> navigateToPageCommand;
        private int selectedSegment;

        public MainViewModel(
            ILogger<MainViewModel> logger,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            this.logger = logger;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }


        public IAsyncRelayCommand AppearingCommand
        {
            get => this.appearingCommand ??= new AsyncRelayCommand(this.OnAppearingAsync);
        }

        private async Task OnAppearingAsync()
        {
            if (!this.isInitialized)
            {
                await this.InitializeAsync();
                this.isInitialized = true;
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                // TODO
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "InitializeAsync failed with exception");
                await this.dialogService.DisplayAlertAsync("Error", "Initialization failed", "OK");
            }
        }

        public IAsyncRelayCommand<string> NavigateToPageCommand
        {
            get => this.navigateToPageCommand ??= new AsyncRelayCommand<string>(this.NavigateToPageAsync);
        }

        private async Task NavigateToPageAsync(string page)
        {
            await this.navigationService.PushAsync(page);
        }

    }
}
