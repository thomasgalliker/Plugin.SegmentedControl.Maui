using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class GenericDetailViewModel : ObservableObject
    {
        private static bool DefaultAnimated = true;

        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        private IAsyncRelayCommand<string> pushCommand;
        private IAsyncRelayCommand<string> pushModalCommand;
        private IAsyncRelayCommand popCommand;
        private IAsyncRelayCommand popModalCommand;
        private IAsyncRelayCommand popToRootCommand;
        private IAsyncRelayCommand navigateCommand;
        private bool animated = DefaultAnimated;

        public GenericDetailViewModel(
            INavigationService navigationService,
            IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public bool Animated
        {
            get => this.animated;
            set
            {
                if (this.SetProperty(ref this.animated, value))
                {
                    DefaultAnimated = value;
                }
            }
        }

        public IAsyncRelayCommand<string> PushCommand
        {
            get => this.pushCommand ??= new AsyncRelayCommand<string>(this.PushAsync);
        }

        private async Task PushAsync(string page)
        {
            try
            {
                await this.navigationService.PushAsync(page, this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public IAsyncRelayCommand<string> PushModalCommand
        {
            get => this.pushModalCommand ??= new AsyncRelayCommand<string>(this.PushModalAsync);
        }

        private async Task PushModalAsync(string page)
        {
            try
            {
                await this.navigationService.PushModalAsync(page, this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public IAsyncRelayCommand PopCommand
        {
            get => this.popCommand ??= new AsyncRelayCommand(this.PopAsync);
        }

        private async Task PopAsync()
        {
            try
            {
                await this.navigationService.PopAsync(this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public IAsyncRelayCommand PopModalCommand
        {
            get => this.popModalCommand ??= new AsyncRelayCommand(this.PopModalAsync);
        }

        private async Task PopModalAsync()
        {
            try
            {
                await this.navigationService.PopModalAsync(this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public IAsyncRelayCommand PopToRootCommand
        {
            get => this.popToRootCommand ??= new AsyncRelayCommand(this.PopToRootAsync);
        }

        private async Task PopToRootAsync()
        {
            try
            {
                await this.navigationService.PopToRootAsync(recursive: true, acrossModals: true, animated: this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

        public IAsyncRelayCommand NavigateCommand
        {
            get => this.navigateCommand ??= new AsyncRelayCommand(this.NavigateAsync);
        }

        private async Task NavigateAsync()
        {
            try
            {
                var navigationPath = "/NavigationPage/MainPage/GenericDetailPage/GenericDetailPage/GenericDetailPage";
                // var navigationPath = "GenericDetailPage/GenericDetailPage/GenericDetailPage";
                await this.navigationService.NavigateAsync(navigationPath, this.Animated);
            }
            catch (Exception ex)
            {
                _ = this.dialogService.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }
    }
}