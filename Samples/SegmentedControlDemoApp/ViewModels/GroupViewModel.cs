using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SegmentedControlDemoApp.ViewModels
{
    public class GroupViewModel : GroupViewModel<object>
    {
        public GroupViewModel(string groupName, ICollection<object> items)
            : base(groupName, items)
        {
        }
    }

    public class GroupViewModel<T> : ObservableCollection<T>
    {
        private IList<T> shadowList = new List<T>();

        private IRelayCommand collapseExpandCommand;
        private bool expanded = true;

        public GroupViewModel(string groupName, ICollection<T> items) : base(items)
        {
            this.GroupName = groupName;
        }

        public string GroupName { get; }

        public bool IsExpanded
        {
            get => this.expanded;
            private set
            {
                this.expanded = value;
                this.OnPropertyChanged(nameof(this.IsExpanded));
            }
        }

        public IRelayCommand CollapseExpandCommand
        {
            get => this.collapseExpandCommand ??= new RelayCommand(this.ToggleExpandCollapse);
        }

        private void ToggleExpandCollapse()
        {
            if (this.IsExpanded)
            {
                this.CollapseInternal();
            }
            else
            {
                this.ExpandInternal();
            }
        }

        public void Collapse()
        {
            if (!this.IsExpanded)
            {
                return;
            }

            this.CollapseInternal();
        }

        public void Expand()
        {
            if (this.IsExpanded)
            {
                return;
            }

            this.ExpandInternal();
        }

        private void CollapseInternal()
        {
            this.shadowList = new List<T>(this);
            this.Clear();
            this.IsExpanded = false;
        }

        private void ExpandInternal()
        {
            foreach (var item in this.shadowList)
            {
                this.Add(item);
            }

            this.shadowList.Clear();
            this.IsExpanded = true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}