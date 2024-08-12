using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace Plugin.SegmentedControl.Maui
{
    [Preserve(AllMembers = true)]
    [ContentProperty(nameof(Children))]
    public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
    {
        public SegmentedControl()
        {
            this.Children = new List<SegmentedControlOption>();
        }

        /// <summary>
        /// Defines if the platform handler is automatically disconnected
        /// or if <c>Handler.DisconnectHandler();</c> is called manually.
        /// Default: <c>true</c> (automatically disconnected)
        /// </summary>
        public bool AutoDisconnectHandler { get; set; } = true;

        public event EventHandler<ChildrenChangingEventArgs> ChildrenChanging;

        public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

        private static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
            nameof(Children),
            typeof(IList<SegmentedControlOption>),
            typeof(SegmentedControl),
            default(IList<SegmentedControlOption>),
            propertyChanging: OnChildrenPropertyChanging);

        public IList<SegmentedControlOption> Children
        {
            get => (IList<SegmentedControlOption>)this.GetValue(ChildrenProperty);
            private set => this.SetValue(ChildrenProperty, value);
        }

        private static void OnChildrenPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SegmentedControl segmentedControl
                && newValue is IList<SegmentedControlOption> newItemsList
                && segmentedControl.Children != null)
            {
                segmentedControl.ChildrenChanging?.Invoke(segmentedControl, new ChildrenChangingEventArgs((IList<SegmentedControlOption>)oldValue, newItemsList));
                segmentedControl.Children.Clear();

                foreach (var newSegment in newItemsList)
                {
                    newSegment.BindingContext = segmentedControl.BindingContext;
                    segmentedControl.Children.Add(newSegment);
                }
            }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(SegmentedControl));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        private void OnItemsSourcePropertyChanged()
        {
            List<SegmentedControlOption> segmentedControlOptions;

            if (this.ItemsSource is IEnumerable<SegmentedControlOption> s)
            {
                segmentedControlOptions = s.ToList();
            }
            else
            {
                var itemsSource = this.ItemsSource;
                var items = itemsSource as IList;

                if (items == null && itemsSource is IEnumerable enumerable)
                {
                    items = enumerable.Cast<object>().ToList();
                }

                if (items != null)
                {
                    var textValues = items as IEnumerable<string>;
                    if (textValues == null && items.Count > 0 && items[0] is string)
                    {
                        textValues = items.Cast<string>();
                    }

                    if (textValues != null)
                    {
                        segmentedControlOptions = textValues
                            .Select(t => new SegmentedControlOption { Text = t })
                            .ToList();
                    }
                    else
                    {
                        segmentedControlOptions = new List<SegmentedControlOption>();
                        var textPropertyName = this.TextPropertyName;
                        foreach (var item in items)
                        {
                            segmentedControlOptions.Add(new SegmentedControlOption
                            {
                                Item = item,
                                TextPropertyName = textPropertyName
                            });
                        }
                    }
                }
                else
                {
                    segmentedControlOptions = new List<SegmentedControlOption>();
                }
            }

            this.Children = segmentedControlOptions;
            this.OnSelectedItemPropertyChanged(true);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.ItemsSource) || propertyName == nameof(this.TextPropertyName))
            {
                this.OnItemsSourcePropertyChanged();
            }
            else if (propertyName == nameof(this.SelectedItem))
            {
                this.OnSelectedItemPropertyChanged();
            }
            else if (propertyName == nameof(this.SelectedSegment))
            {
                this.OnSelectedSegmentPropertyChanged();
            }
        }

        private void OnSelectedSegmentPropertyChanged()
        {
            var segmentIndex = this.SelectedSegment;
            if (segmentIndex >= 0 && segmentIndex < this.Children.Count && this.SelectedItem != this.Children[segmentIndex].Item)
            {
                this.SelectedItem = this.Children[segmentIndex].Item;
            }
        }

        private void OnSelectedItemPropertyChanged(bool forceUpdateSelectedSegment = false)
        {
            if (this.TextPropertyName != null)
            {
                var selectedItem = this.SelectedItem;
                var selectedIndex = this.Children.IndexOf(item => item.Item == selectedItem);
                if (selectedIndex == -1)
                {
                    selectedIndex = this.SelectedSegment;
                    if (selectedIndex < 0 || selectedIndex >= this.Children.Count)
                    {
                        this.SelectedSegment = 0;
                    }
                    else if (this.SelectedSegment != selectedIndex)
                    {
                        this.SelectedSegment = selectedIndex;
                    }
                    else if (forceUpdateSelectedSegment)
                    {
                        this.OnSelectedSegmentPropertyChanged();
                    }
                }
                else if (selectedIndex != this.SelectedSegment)
                {
                    this.SelectedSegment = selectedIndex;
                }
            }
        }

        public static readonly BindableProperty TextPropertyNameProperty = BindableProperty.Create(
            nameof(TextPropertyName),
            typeof(string),
            typeof(SegmentedControl));

        public string TextPropertyName
        {
            get => (string)this.GetValue(TextPropertyNameProperty);
            set => this.SetValue(TextPropertyNameProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Black);

        public Color TextColor
        {
            get => (Color)this.GetValue(TextColorProperty);
            set => this.SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Blue);

        public Color TintColor
        {
            get => (Color)this.GetValue(TintColorProperty);
            set => this.SetValue(TintColorProperty, value);
        }

        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(
            nameof(SelectedTextColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.White);

        public Color SelectedTextColor
        {
            get => (Color)this.GetValue(SelectedTextColorProperty);
            set => this.SetValue(SelectedTextColorProperty, value);
        }

        public static readonly BindableProperty DisabledBackgroundColorProperty = BindableProperty.Create(
            nameof(DisabledBackgroundColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Gray);

        public Color DisabledBackgroundColor
        {
            get => (Color)this.GetValue(DisabledBackgroundColorProperty);
            set => this.SetValue(DisabledBackgroundColorProperty, value);
        }

        public static readonly BindableProperty DisabledTintColorProperty = BindableProperty.Create(
            nameof(DisabledTintColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Gray);

        public Color DisabledTintColor
        {
            get => (Color)this.GetValue(DisabledTintColorProperty);
            set => this.SetValue(DisabledTintColorProperty, value);
        }

        public static readonly BindableProperty DisabledTextColorProperty = BindableProperty.Create(
            nameof(DisabledTextColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Gray);

        public Color DisabledTextColor
        {
            get => (Color)this.GetValue(DisabledTextColorProperty);
            set => this.SetValue(DisabledTextColorProperty, value);
        }

        public static readonly BindableProperty DisabledSelectedTextColorProperty = BindableProperty.Create(
            nameof(DisabledSelectedTextColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.LightGray);

        public Color DisabledSelectedTextColor
        {
            get => (Color)this.GetValue(DisabledSelectedTextColorProperty);
            set => this.SetValue(DisabledSelectedTextColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(SegmentedControl),
            defaultValueCreator: bindable => ((SegmentedControl)bindable).TintColor);

        public Color BorderColor
        {
            get => (Color)this.GetValue(BorderColorProperty);
            set => this.SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
            nameof(BorderWidth),
            typeof(double),
            typeof(SegmentedControl),
            defaultValueCreator: _ => DeviceInfo.Platform == DevicePlatform.Android ? 1.0 : 0.0);

        public double BorderWidth
        {
            get => (double)this.GetValue(BorderWidthProperty);
            set => this.SetValue(BorderWidthProperty, value);
        }

        public static readonly BindableProperty SelectedSegmentProperty = BindableProperty.Create(
            nameof(SelectedSegment),
            typeof(int),
            typeof(SegmentedControl),
            0,
            BindingMode.TwoWay);

        public int SelectedSegment
        {
            get => (int)this.GetValue(SelectedSegmentProperty);
            set => this.SetValue(SelectedSegmentProperty, value);
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(SegmentedControl),
            null,
            BindingMode.TwoWay);

        public object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        public static readonly BindableProperty SegmentSelectedCommandProperty = BindableProperty.Create(
            nameof(SegmentSelectedCommand),
            typeof(ICommand),
            typeof(SegmentedControl));

        public ICommand SegmentSelectedCommand
        {
            get => (ICommand)this.GetValue(SegmentSelectedCommandProperty);
            set => this.SetValue(SegmentSelectedCommandProperty, value);
        }

        public static readonly BindableProperty SegmentSelectedCommandParameterProperty = BindableProperty.Create(
            nameof(SegmentSelectedCommandParameter),
            typeof(object),
            typeof(SegmentedControl));

        public object SegmentSelectedCommandParameter
        {
            get => this.GetValue(SegmentSelectedCommandParameterProperty);
            set => this.SetValue(SegmentSelectedCommandParameterProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(SegmentedControl),
            0d);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(SegmentedControl));

        public string FontFamily
        {
            get => (string)this.GetValue(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(SegmentedControl),
            FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)this.GetValue(FontAttributesProperty);
            set => this.SetValue(FontAttributesProperty, value);
        }

        internal void RaiseSelectionChanged(int segment)
        {
            if (this.IsEnabled &&
                segment >= 0 &&
                segment < this.Children.Count &&
                this.Children[segment].IsEnabled)
            {
                SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs { NewValue = segment });

                if (!(this.SegmentSelectedCommand is null) && this.SegmentSelectedCommand.CanExecute(this.SegmentSelectedCommandParameter))
                {
                    this.SegmentSelectedCommand.Execute(this.SegmentSelectedCommandParameter);
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.Children is not null)
            {
                foreach (var segmentedControlOption in this.Children)
                {
                    segmentedControlOption.BindingContext = this.BindingContext;
                }
            }
        }
    }
}
