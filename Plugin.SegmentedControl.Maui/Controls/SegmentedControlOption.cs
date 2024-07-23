using System.ComponentModel;

namespace Plugin.SegmentedControl.Maui
{
    [Preserve(AllMembers = true)]
    public class SegmentedControlOption : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(SegmentedControlOption),
            string.Empty);

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextPropertyNameProperty = BindableProperty.Create(
            nameof(TextPropertyName),
            typeof(string),
            typeof(SegmentedControlOption));

        public string TextPropertyName
        {
            get => (string)this.GetValue(TextPropertyNameProperty);
            set => this.SetValue(TextPropertyNameProperty, value);
        }

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            nameof(Item),
            typeof(object),
            typeof(SegmentedControlOption),
            propertyChanged: (bindable, oldValue, newValue) => ((SegmentedControlOption)bindable).OnItemPropertyChanged(oldValue, newValue));

        public object Item
        {
            get => this.GetValue(ItemProperty);
            set => this.SetValue(ItemProperty, value);
        }

        private void OnItemPropertyChanged(object oldValue, object newValue)
        {
            if (oldValue is INotifyPropertyChanged mutableItem)
            {
                mutableItem.PropertyChanged -= this.OnItemPropertyChanged;
            }

            if (newValue is INotifyPropertyChanged newMutableItem)
            {
                newMutableItem.PropertyChanged += this.OnItemPropertyChanged;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.Item) ||
                propertyName == nameof(this.TextPropertyName))
            {
                this.SetTextFromItemProperty();
            }
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.TextPropertyName)
            {
                this.SetTextFromItemProperty();
            }
        }

        private void SetTextFromItemProperty()
        {
            if (this.Item != null && this.TextPropertyName != null)
            {
                var propertyInfo = this.Item.GetType().GetProperty(this.TextPropertyName);
                this.Text = propertyInfo?.GetValue(this.Item)?.ToString();
            }
        }
    }
}
