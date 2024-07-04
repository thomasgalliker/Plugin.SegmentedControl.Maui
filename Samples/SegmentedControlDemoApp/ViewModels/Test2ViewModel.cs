using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SegmentedControlDemoApp.ViewModels
{
    public class Test2ViewModel : ObservableObject
    {
        private int selectedSegment;
        private string[] colors;

        public Test2ViewModel()
        {
            this.Colors =
            [
                "Red",
                "Green",
                "Blue",
            ];
        }

        public string[] Colors
        {
            get => this.colors;
            private set => this.SetProperty(ref this.colors, value);
        }

        public int SelectedSegment
        {
            get => this.selectedSegment;
            set
            {
                if (this.SetProperty(ref this.selectedSegment, value))
                {
                    this.OnPropertyChanged(nameof(this.IsTab1Active));
                    this.OnPropertyChanged(nameof(this.IsTab2Active));
                    this.OnPropertyChanged(nameof(this.IsTab3Active));
                }
            }
        }

        public bool IsTab1Active => this.SelectedSegment == 0;

        public bool IsTab2Active => this.SelectedSegment == 1;

        public bool IsTab3Active => this.SelectedSegment == 2;
    }
}
