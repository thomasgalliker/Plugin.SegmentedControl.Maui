using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        private IAsyncRelayCommand<string> navigateToPageCommand;

        public MainViewModel(
            INavigationService navigationService)
        {
            this.navigationService = navigationService;
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
