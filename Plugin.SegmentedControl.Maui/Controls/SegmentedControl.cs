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

        public event EventHandler<ElementChildrenChanging> OnElementChildrenChanging;

        public event EventHandler<SegmentSelectEventArgs> OnSegmentSelected;

        public static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
            nameof(Children),
            typeof(IList<SegmentedControlOption>),
            typeof(SegmentedControl),
            default(IList<SegmentedControlOption>),
            propertyChanging: OnChildrenPropertyChanging);

        public IList<SegmentedControlOption> Children
        {
            get => (IList<SegmentedControlOption>)this.GetValue(ChildrenProperty);
            set => this.SetValue(ChildrenProperty, value);
        }

        private static void OnChildrenPropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SegmentedControl segmentedControl
                && newValue is IList<SegmentedControlOption> newItemsList
                && segmentedControl.Children != null)
            {
                segmentedControl.OnElementChildrenChanging?.Invoke(segmentedControl, new ElementChildrenChanging((IList<SegmentedControlOption>)oldValue, newItemsList));
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

        private void OnItemsSourceChanged()
        {
            var itemsSource = this.ItemsSource;
            var items = itemsSource as IList;
            if (items == null && itemsSource is IEnumerable list)
            {
                items = list.Cast<object>().ToList();
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
                    this.Children = new List<SegmentedControlOption>(textValues.Select(child => new SegmentedControlOption { Text = child }));
                    this.OnSelectedItemChanged(true);
                }
                else
                {
                    var textPropertyName = this.TextPropertyName;
                    if (textPropertyName != null)
                    {
                        var newChildren = new List<SegmentedControlOption>();
                        foreach (var item in items)
                        {
                            newChildren.Add(new SegmentedControlOption { Item = item, TextPropertyName = textPropertyName });
                        }

                        this.Children = newChildren;
                        this.OnSelectedItemChanged(true);
                    }
                }
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.ItemsSource) || propertyName == nameof(this.TextPropertyName))
            {
                this.OnItemsSourceChanged();
            }
            else if (propertyName == nameof(this.SelectedItem))
            {
                this.OnSelectedItemChanged();
            }
            else if (propertyName == nameof(this.SelectedSegment))
            {
                this.OnSelectedSegmentChanged();
            }
        }

        private void OnSelectedSegmentChanged()
        {
            var segmentIndex = this.SelectedSegment;
            if (segmentIndex >= 0 && segmentIndex < this.Children.Count && this.SelectedItem != this.Children[segmentIndex].Item)
            {
                this.SelectedItem = this.Children[segmentIndex].Item;
            }
        }

        private void OnSelectedItemChanged(bool forceUpdateSelectedSegment = false)
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
                        this.OnSelectedSegmentChanged();
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

        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create(
            nameof(DisabledColor),
            typeof(Color),
            typeof(SegmentedControl),
            Colors.Gray);

        public Color DisabledColor
        {
            get => (Color)this.GetValue(DisabledColorProperty);
            set => this.SetValue(DisabledColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor), typeof(Color), typeof(SegmentedControl), defaultValueCreator: bindable => ((SegmentedControl)bindable).TintColor);

        public Color BorderColor
        {
            get => (Color)this.GetValue(BorderColorProperty);
            set => this.SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
            nameof(BorderWidth),
            typeof(double),
            typeof(SegmentedControl),
            defaultValueCreator: _ => Device.RuntimePlatform == Device.Android ? 1.0 : 0.0);

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
            Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

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

        internal void RaiseSelectionChanged()
        {
            OnSegmentSelected?.Invoke(this, new SegmentSelectEventArgs { NewValue = this.SelectedSegment });

            if (!(this.SegmentSelectedCommand is null) && this.SegmentSelectedCommand.CanExecute(this.SegmentSelectedCommandParameter))
            {
                this.SegmentSelectedCommand.Execute(this.SegmentSelectedCommandParameter);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.Children is not null)
            {
                foreach (var segment in this.Children)
                {
                    segment.BindingContext = this.BindingContext;
                }
            }
        }
    }
}
