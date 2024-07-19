using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.Views.ViewGroup;
using RadioButton = Android.Widget.RadioButton;

namespace Plugin.SegmentedControl.Maui
{
    public class SegmentedControlHandler : ViewHandler<SegmentedControl, RadioGroup>
    {
        private RadioButton selectedRadioButton;

        public static IPropertyMapper<SegmentedControl, SegmentedControlHandler> Mapper = new PropertyMapper<SegmentedControl, SegmentedControlHandler>(ViewMapper)
        {
            [nameof(SegmentedControl.IsEnabled)] = MapIsEnabled,
            [nameof(SegmentedControl.SelectedSegment)] = MapSelectedSegment,
            [nameof(SegmentedControl.TintColor)] = MapTintColor,
            [nameof(SegmentedControl.TextColor)] = MapTextColor,
            [nameof(SegmentedControl.SelectedTextColor)] = MapSelectedTextColor,
            [nameof(SegmentedControl.DisabledBackgroundColor)] = MapDisabledBackgroundColor,
            [nameof(SegmentedControl.DisabledTextColor)] = MapDisabledTextColor,
            [nameof(SegmentedControl.DisabledTintColor)] = MapDisabledTintColor,
            [nameof(SegmentedControl.Children)] = MapChildren,
        };

        public SegmentedControlHandler() : base(Mapper)
        {
        }

        public SegmentedControlHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {
        }

        protected override RadioGroup CreatePlatformView()
        {
            var layoutInflater = LayoutInflater.From(this.Context);

            var nativeControl = (RadioGroup)layoutInflater.Inflate(Resource.Layout.RadioGroup, null);

            for (var i = 0; i < this.VirtualView.Children.Count; i++)
            {
                var o = this.VirtualView.Children[i];
                var isButtonEnabled = this.VirtualView.IsEnabled && o.IsEnabled;
                var rb = (RadioButton)layoutInflater.Inflate(Resource.Layout.RadioButton, null);

                rb.LayoutParameters = new RadioGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent, 1f);
                rb.Text = o.Text;

                if (i == 0)
                {
                    rb.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                }
                else if (i == this.VirtualView.Children.Count - 1)
                {
                    rb.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                }

                this.ConfigureRadioButton(i, isButtonEnabled, rb);

                nativeControl.AddView(rb);
            }

            {
                var option = (RadioButton)nativeControl.GetChildAt(this.VirtualView.SelectedSegment);

                if (option != null)
                {
                    option.Checked = true;
                }
            }

            return nativeControl;
        }

        internal static bool NeedsExactMeasure(IView virtualView)
        {
            if (virtualView.VerticalLayoutAlignment != Microsoft.Maui.Primitives.LayoutAlignment.Fill
                && virtualView.HorizontalLayoutAlignment != Microsoft.Maui.Primitives.LayoutAlignment.Fill)
            {
                // Layout Alignments of Start, Center, and End will be laying out the TextView at its measured size,
                // so we won't need another pass with MeasureSpecMode.Exactly
                return false;
            }

            if (virtualView.Width >= 0 && virtualView.Height >= 0)
            {
                // If the Width and Height are both explicit, then we've already done MeasureSpecMode.Exactly in 
                // both dimensions; no need to do it again
                return false;
            }

            // We're going to need a second measurement pass so TextView can properly handle alignments
            return true;
        }


        internal static int MakeMeasureSpecExact(RadioGroup view, double size)
        {
            // Convert to a native size to create the spec for measuring
            var deviceSize = (int)view.Context.ToPixels(size);
            return MeasureSpecMode.Exactly.MakeMeasureSpec(deviceSize);
        }

        internal void PrepareArrange(Rect frame)
        {
            if (frame.Width < 0 || frame.Height < 0)
            {
                return;
            }

            RadioGroup platformView = this.PlatformView;
            if (platformView == null)
            {
                return;
            }

            var virtualView = this.VirtualView;
            if (virtualView == null)
            {
                return;
            }

            // Depending on our layout situation, the TextView may need an additional measurement pass at the final size
            // in order to properly handle any TextAlignment properties and some internal bookkeeping
            if (NeedsExactMeasure(virtualView))
            {
                platformView.Measure(
                    MakeMeasureSpecExact(platformView, frame.Width),
                    MakeMeasureSpecExact(platformView, frame.Height));
            }
        }

        public override void PlatformArrange(Rect frame)
        {
            this.PrepareArrange(frame);
            base.PlatformArrange(frame);
        }

        protected override void ConnectHandler(RadioGroup platformView)
        {
            base.ConnectHandler(platformView);

            platformView.CheckedChange += this.PlatformView_CheckedChange;
        }

        protected override void DisconnectHandler(RadioGroup platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.CheckedChange -= this.PlatformView_CheckedChange;
            this.selectedRadioButton = null;
        }

        void PlatformView_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var rg = (RadioGroup)sender;
            if (rg.CheckedRadioButtonId != -1)
            {
                var id = rg.CheckedRadioButtonId;
                var radioButton = rg.FindViewById(id);
                var radioId = rg.IndexOfChild(radioButton);

                var rb = (RadioButton)rg.GetChildAt(radioId);

                //set newly selected button properties
                var isNewButtonEnabled = this.VirtualView.IsEnabled && rb.Enabled;

                var selectedTextColor = isNewButtonEnabled ?
                    this.VirtualView.SelectedTextColor.ToPlatform() :
                    this.VirtualView.DisabledTextColor.ToPlatform();

                var selectedTintColor = isNewButtonEnabled ?
                    this.VirtualView.TintColor.ToPlatform() :
                    this.VirtualView.DisabledTintColor.ToPlatform();

                rb.SetTextColor(selectedTextColor);
                this.SetTintColor(rb, selectedTintColor);

                //reset old selected button properties
                if (this.selectedRadioButton != null && this.selectedRadioButton.Id != rb.Id)
                {
                    var isOldButtonEnabled = this.selectedRadioButton.Enabled;
                    var textColor = isOldButtonEnabled ?
                        this.VirtualView.TextColor.ToPlatform() :
                        this.VirtualView.DisabledTextColor.ToPlatform();

                    var tintColor = isOldButtonEnabled ?
                        this.VirtualView.TintColor.ToPlatform() :
                        this.VirtualView.DisabledBackgroundColor.ToPlatform();

                    this.selectedRadioButton.SetTextColor(textColor);
                    this.SetTintColor(this.selectedRadioButton, tintColor);
                }

                this.selectedRadioButton = rb;

                this.VirtualView.SelectedSegment = radioId;
            }
        }

        void ConfigureRadioButton(int i, bool isEnabled, RadioButton rb)
        {
            var isButtonEnabled = this.VirtualView.IsEnabled && isEnabled;

            if (rb.Enabled != isButtonEnabled)
            {
                rb.Enabled = isButtonEnabled;
            }

            var isSelected = i == this.VirtualView.SelectedSegment;

            var tintColor = this.GetTintColor(isSelected, isButtonEnabled);

            if (i == this.VirtualView.SelectedSegment)
            {
                var selectedTextColor = isButtonEnabled
                    ? this.VirtualView.SelectedTextColor.ToPlatform()
                    : this.VirtualView.DisabledTextColor.ToPlatform();

                rb.SetTextColor(selectedTextColor);
                this.selectedRadioButton = rb;
            }
            else
            {
                var textColor = isButtonEnabled
                    ? this.VirtualView.TextColor.ToPlatform()
                    : this.VirtualView.DisabledTextColor.ToPlatform();

                rb.SetTextColor(textColor);
            }

            this.SetTintColor(rb, tintColor);

            rb.Tag = i;
            rb.Click += this.RadioButton_Click;
        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
            if (sender is RadioButton rb)
            {
                var t = (int)rb.Tag;
                this.VirtualView.RaiseSelectionChanged(t);
            }
        }

        private Android.Graphics.Color GetTintColor(bool selected, bool enabled)
        {
            return enabled ?
                this.VirtualView.TintColor.ToPlatform() :
                this.VirtualView.DisabledTintColor.ToPlatform();

            // 'tint' is an outline + selected button color, so 
            //the backgroundcolor for the segmented control can't be used as 'tint'

            //TODO we should have a separate outline color 
            // and ability to pick a background color for selected(checked) segment
        }

        private void SetTintColor(RadioButton rb, Android.Graphics.Color tintColor)
        {
            GradientDrawable selectedShape;
            GradientDrawable unselectedShape;

            //do not call SetBackgroundColor, that sets the state to ColorDrawable & makes invalid cast
            var gradientDrawable = (StateListDrawable)rb.Background;
            var drawableContainerState = (DrawableContainer.DrawableContainerState)gradientDrawable.GetConstantState();
            var children = drawableContainerState.GetChildren();

            // Doesnt works on API < 18
            selectedShape = children[0] is GradientDrawable drawable ? drawable : (GradientDrawable)((InsetDrawable)children[0]).Drawable;
            unselectedShape = children[1] is GradientDrawable drawable1 ? drawable1 : (GradientDrawable)((InsetDrawable)children[1]).Drawable;

            selectedShape.SetStroke(3, tintColor);
            selectedShape.SetColor(tintColor);
            unselectedShape.SetStroke(3, tintColor);
        }

        static void MapTintColor(SegmentedControlHandler handler, SegmentedControl control) => OnPropertyChanged(handler, control);

        static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl control)
        {
            var option = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);

            if (option != null)
            {
                option.Checked = true;
            }

            var t = (int)option.Tag;
            control.RaiseSelectionChanged(t);
        }


        static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl control) => OnPropertyChanged(handler, control);

        static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            var v = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);
            v?.SetTextColor(control.SelectedTextColor.ToPlatform());
        }

        static void MapDisabledTintColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (control.SelectedSegment < 0)
            {
                return;
            }

            var v = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);

            var isButtonEnabled = control.IsEnabled && v.Enabled;
            var tintColor = handler.GetTintColor(true, isButtonEnabled);
            handler.SetTintColor(v, tintColor);
        }

        private static void MapDisabledTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            // Go through tab items and update disabled segments
            var childCount = handler.PlatformView.ChildCount;
            if (childCount > 0)
            {
                var disabledTextColor = control.DisabledTextColor.ToPlatform();

                for (var i = 0; i < childCount; i++)
                {
                    var radioButton = (RadioButton)handler.PlatformView.GetChildAt(i);
                    if (radioButton != null && (!radioButton.Enabled || !control.IsEnabled))
                    {
                        radioButton.SetTextColor(disabledTextColor);
                    }
                }
            }
        }

        static void MapDisabledBackgroundColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            //go through children and update disabled segments
            for (int i = 0; i < handler.PlatformView.ChildCount; i++)
            {
                var v = (RadioButton)handler.PlatformView.GetChildAt(i);
                var tintColor = handler.GetTintColor(v.Checked, !v.Enabled || !control.IsEnabled);
                handler.SetTintColor(v, tintColor);
            }
        }

        static void MapChildren(SegmentedControlHandler handler, SegmentedControl control)
        {
            //entire Children property has been changed -- woo hoo we essentialy have to
            //re-create all the segments now

            if (handler.PlatformView != null && control != null)
            {
                //first, remove old children
                var radioGroup = handler.PlatformView;

                var count = radioGroup.ChildCount;
                if (count > 0)
                {
                    for (var i = count - 1; i >= 0; i--)
                    {
                        var o = radioGroup.GetChildAt(i);
                        if (o is RadioButton)
                        {
                            radioGroup.RemoveViewAt(i);
                        }
                    }

                    //next, add new children
                    var layoutInflater = LayoutInflater.From(handler.Context);

                    var vv = handler.VirtualView;
                    for (var i = 0; i < vv.Children.Count; i++)
                    {
                        var o = vv.Children[i];
                        var isButtonEnabled = vv.IsEnabled && o.IsEnabled;
                        var rb = (RadioButton)layoutInflater.Inflate(
                            Resource.Layout.RadioButton, null);

                        rb.LayoutParameters = new RadioGroup.LayoutParams(
                            LayoutParams.MatchParent, LayoutParams.WrapContent, 1f);
                        rb.Text = o.Text;

                        if (i == 0)
                        {
                            rb.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                        }
                        else if (i == vv.Children.Count - 1)
                        {
                            rb.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                        }

                        handler.ConfigureRadioButton(i, isButtonEnabled, rb);
                        radioGroup.AddView(rb);
                    }
                    var option = (RadioButton)radioGroup.GetChildAt(vv.SelectedSegment);
                    if (option != null)
                    {
                        option.Checked = true;
                    }
                }
            }
        }

        static void MapTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            for (int i = 0; i < handler.PlatformView.ChildCount; i++)
            {
                var v = (RadioButton)handler.PlatformView.GetChildAt(i);
                if (i != control.SelectedSegment)
                {
                    v.SetTextColor(control.TextColor.ToPlatform());
                }
                else
                {
                    v.SetTextColor(control.SelectedTextColor.ToPlatform());
                }
            }
        }

        static void OnPropertyChanged(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (handler.PlatformView != null && control != null)
            {
                for (var i = 0; i < control.Children.Count; i++)
                {
                    var child = control.Children[i];
                    var rb = (RadioButton)handler.PlatformView.GetChildAt(i);
                    if (rb.Text != child.Text)
                    {
                        rb.Text = child.Text;
                    }

                    handler.ConfigureRadioButton(i, control.Children[i].IsEnabled, rb);
                }
            }
        }
    }
}
