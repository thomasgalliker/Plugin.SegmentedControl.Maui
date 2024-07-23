using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly ILauncher launcher;

        private IAsyncRelayCommand<string> navigateToPageCommand;
        private IAsyncRelayCommand<string> openUrlCommand;

        public MainViewModel(
            INavigationService navigationService,
            ILauncher launcher)
        {
            this.navigationService = navigationService;
            this.launcher = launcher;
        }

        public IAsyncRelayCommand<string> NavigateToPageCommand
        {
            get => this.navigateToPageCommand ??= new AsyncRelayCommand<string>(this.NavigateToPageAsync);
        }

        private async Task NavigateToPageAsync(string page)
        {
            await this.navigationService.PushAsync(page);
        }


        public IAsyncRelayCommand<string> OpenUrlCommand
        {
            get => this.openUrlCommand ??= new AsyncRelayCommand<string>(this.OpenUrlAsync);
        }

        private async Task OpenUrlAsync(string url)
        {
            try
            {
                await this.launcher.TryOpenAsync(url);
            }
            catch
            {
                // Ignore exceptions
            }
        }
    }
}
