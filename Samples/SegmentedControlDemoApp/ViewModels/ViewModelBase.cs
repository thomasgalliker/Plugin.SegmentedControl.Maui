using CommunityToolkit.Mvvm.ComponentModel;

namespace SegmentedControlDemoApp.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                if (this.SetProperty(ref this.isBusy, value))
                {
                    this.OnPropertyChanged(nameof(this.IsNotBusy));
                }
            }
        }

        public bool IsNotBusy => !this.IsBusy;
    }
}