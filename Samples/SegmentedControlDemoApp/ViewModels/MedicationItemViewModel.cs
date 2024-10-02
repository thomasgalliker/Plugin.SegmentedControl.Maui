using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MedicationItemViewModel
    {
        private readonly INavigationService navigationService;

        private IAsyncRelayCommand navigateGenericDetailPageCommand;

        public MedicationItemViewModel(INavigationService navigationService, string name)
        {
            this.navigationService = navigationService;
            this.Name = name;
        }

        public string Name { get; }

        public IAsyncRelayCommand NavigateGenericDetailPageCommand
        {
            get => this.navigateGenericDetailPageCommand ??= new AsyncRelayCommand(this.NavigateToGenericDetailPageAsync);
        }

        private async Task NavigateToGenericDetailPageAsync()
        {
            await this.navigationService.PushAsync("GenericDetailPage");
        }
    }
}