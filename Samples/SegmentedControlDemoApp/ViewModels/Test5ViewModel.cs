using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SegmentedControlDemoApp.Services;

namespace SegmentedControlDemoApp.ViewModels
{
    public class Test5ViewModel : ViewModelBase
    {
        private readonly ILogger logger;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;

        private bool isInitialized;
        private int selectedSegment;
        private IAsyncRelayCommand addMedicationCommand;
        private IAsyncRelayCommand appearingCommand;
        private ICollection<GroupViewModel<MedicationItemViewModel>> activeMedications;
        private ICollection<GroupViewModel<MedicationItemViewModel>> futureMedications;
        private ICollection<GroupViewModel<MedicationItemViewModel>> pastMedications;

        public Test5ViewModel(
            ILogger<Test5ViewModel> logger,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            this.logger = logger;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            this.ActiveMedications = Array.Empty<GroupViewModel<MedicationItemViewModel>>();
            this.FutureMedications = Array.Empty<GroupViewModel<MedicationItemViewModel>>();
            this.PastMedications = Array.Empty<GroupViewModel<MedicationItemViewModel>>();
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
                this.ActiveMedications = new List<GroupViewModel<MedicationItemViewModel>>
                {
                    new("Class 1a",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Ajmaline"),
                            this.CreateMedicationItemViewModel("Disopyramide"),
                            this.CreateMedicationItemViewModel("Procainamide"),
                        }),
                    new("Class 1b",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Lidocaine"),
                            this.CreateMedicationItemViewModel("Mexiletine"),
                            this.CreateMedicationItemViewModel("Phenytoin"),
                        }),
                    new("Class 1c",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Encainide"),
                            this.CreateMedicationItemViewModel("Flecainide"),
                            this.CreateMedicationItemViewModel("Moricizine"),
                            this.CreateMedicationItemViewModel("Propafenone"),
                        }),
                };

                this.FutureMedications = new List<GroupViewModel<MedicationItemViewModel>>
                {
                    new("Class 2",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Atenolol"),
                            this.CreateMedicationItemViewModel("Bisoprolol"),
                            this.CreateMedicationItemViewModel("Carvedilol"),
                            this.CreateMedicationItemViewModel("Esmolol"),
                        }),
                };

                this.PastMedications = new List<GroupViewModel<MedicationItemViewModel>>
                {
                    new("Class 3",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Amiodarone"),
                            this.CreateMedicationItemViewModel("Dofetilide"),
                            this.CreateMedicationItemViewModel("Dronedarone"),
                        }),
                    new("Class 4",
                        new List<MedicationItemViewModel>
                        {
                            this.CreateMedicationItemViewModel("Diltiazem"),
                            this.CreateMedicationItemViewModel("Verapamil"),
                        }),
                };

                this.SelectedSegment = 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "InitializeAsync failed with exception");
                await this.dialogService.DisplayAlertAsync("Error", "Initialization failed", "OK");
            }
        }

        private MedicationItemViewModel CreateMedicationItemViewModel(string name)
        {
            return new(this.navigationService, name);
        }

        public ICollection<GroupViewModel<MedicationItemViewModel>> ActiveMedications
        {
            get => this.activeMedications;
            private set
            {
                if (this.SetProperty(ref this.activeMedications, value))
                {
                    this.OnPropertyChanged(nameof(this.ActiveMedicationsLabelText));
                }
            }
        }

        public ICollection<GroupViewModel<MedicationItemViewModel>> FutureMedications
        {
            get => this.futureMedications;
            private set
            {
                if (this.SetProperty(ref this.futureMedications, value))
                {
                    this.OnPropertyChanged(nameof(this.FutureMedicationsLabelText));
                }
            }
        }

        public ICollection<GroupViewModel<MedicationItemViewModel>> PastMedications
        {
            get => this.pastMedications;
            private set
            {
                if (this.SetProperty(ref this.pastMedications, value))
                {
                    this.OnPropertyChanged(nameof(this.PastMedicationsLabelText));
                }
            }
        }

        public string ActiveMedicationsLabelText
        {
            get => $"Class 1 ({this.ActiveMedications.Count})";
        }

        public string FutureMedicationsLabelText
        {
            get => $"Class 2 ({this.FutureMedications.Count})";
        }

        public string PastMedicationsLabelText
        {
            get => $"Class 3+ ({this.PastMedications.Count})";
        }

        public int SelectedSegment
        {
            get => this.selectedSegment;
            set
            {
                if (this.SetProperty(ref this.selectedSegment, value))
                {
                    this.OnPropertyChanged(nameof(this.IsActiveMedicationsSelected));
                    this.OnPropertyChanged(nameof(this.IsFutureMedicationsSelected));
                    this.OnPropertyChanged(nameof(this.IsPastMedicationsSelected));
                }
            }
        }

        public bool IsActiveMedicationsSelected => this.SelectedSegment == 0;

        public bool IsFutureMedicationsSelected => this.SelectedSegment == 1;

        public bool IsPastMedicationsSelected => this.SelectedSegment == 2;

        public IAsyncRelayCommand AddMedicationCommand
        {
            get => this.addMedicationCommand ??= new AsyncRelayCommand(this.AddMedicationAsync);
        }

        private async Task AddMedicationAsync()
        {
            await this.navigationService.PushModalAsync("Test5DetailPage");
        }
    }
}