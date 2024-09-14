using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MedicationDetailViewModel
    {
        private readonly INavigationService navigationService;

        private IAsyncRelayCommand popToRootCommand;

        public MedicationDetailViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public IAsyncRelayCommand PopToRootCommand
        {
            get => this.popToRootCommand ??= new AsyncRelayCommand(this.PopToRootAsync);
        }

        private async Task PopToRootAsync()
        {
            await this.navigationService.PopToRootAsync();
        }
    }
}