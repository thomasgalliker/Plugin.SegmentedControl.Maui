using CommunityToolkit.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SegmentedControlTest.ViewModels
{

    public class Modal1ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>
        {
            "Item 1",
            "Item 2",
            "Item 3"
        };
        
        public Modal1ViewModel()
        {
            // Initialize properties or commands here if needed
        }


        private string _exampleProperty = "Initial Value";
        public string ExampleProperty
        {
            get => _exampleProperty;
            set
            {
                if (_exampleProperty != value)
                {
                    _exampleProperty = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isImageVisible = true;
        public bool IsImageVisible
        {
            get => DateTime.Now.Second % 2 == 0 ? true : false;
            set
            {
                _isImageVisible = value;                
                OnPropertyChanged();                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}