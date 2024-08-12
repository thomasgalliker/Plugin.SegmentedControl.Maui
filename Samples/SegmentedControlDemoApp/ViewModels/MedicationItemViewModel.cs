using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class MedicationItemViewModel
    {
        private readonly INavigationService navigationService;

        private IAsyncRelayCommand navigateMedicationDetailPageCommand;

        public MedicationItemViewModel(INavigationService navigationService, string name)
        {
            this.navigationService = navigationService;
            this.Name = name;
        }

        public string Name { get; }

        public IAsyncRelayCommand NavigateMedicationDetailPageCommand
        {
            get => this.navigateMedicationDetailPageCommand ??= new AsyncRelayCommand(this.NavigateToMedicationDetailPageAsync);
        }

        private async Task NavigateToMedicationDetailPageAsync()
        {
            await this.navigationService.PushAsync("MedicationDetailPage");
        }
    }
}