using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Plugin.SegmentedControl.Maui.Extensions;
using static Android.Views.ViewGroup;
using Font = Microsoft.Maui.Font;
using RadioButton = Android.Widget.RadioButton;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace Plugin.SegmentedControl.Maui
{
    public class SegmentedControlHandler : ViewHandler<SegmentedControl, RadioGroup>
    {
        private RadioButton selectedRadioButton;

        public static IPropertyMapper<SegmentedControl, SegmentedControlHandler> Mapper =
            new PropertyMapper<SegmentedControl, SegmentedControlHandler>(ViewMapper)
            {
                [nameof(SegmentedControl.IsEnabled)] = MapIsEnabled,
                [nameof(SegmentedControl.SelectedSegment)] = MapSelectedSegment,
                [nameof(SegmentedControl.TintColor)] = MapTintColor,
                [nameof(SegmentedControl.DisabledTintColor)] = MapDisabledTintColor,
                [nameof(SegmentedControl.TextColor)] = MapTextColor,
                [nameof(SegmentedControl.DisabledTextColor)] = MapDisabledTextColor,
                [nameof(SegmentedControl.SelectedTextColor)] = MapSelectedTextColor,
                [nameof(SegmentedControl.DisabledSelectedTextColor)] = MapDisabledSelectedTextColor,
                [nameof(SegmentedControl.DisabledBackgroundColor)] = MapDisabledBackgroundColor,
                [nameof(SegmentedControl.FontFamily)] = MapFontFamily,
                [nameof(SegmentedControl.FontSize)] = MapFontSize,
                [nameof(SegmentedControl.FontAttributes)] = MapFontAttributes,
                [nameof(SegmentedControl.Children)] = MapChildren,
            };

        public SegmentedControlHandler() : base(Mapper)
        {
        }

        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);
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

            var platformView = this.PlatformView;
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

            if (this.VirtualView.AutoDisconnectHandler)
            {
                this.VirtualView.AddCleanUpEvent();
            }

            platformView.CheckedChange += this.PlatformView_CheckedChange;
        }

        protected override void DisconnectHandler(RadioGroup platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.CheckedChange -= this.PlatformView_CheckedChange;
            this.selectedRadioButton = null;
        }

        private void PlatformView_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
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

                var selectedTextColor = isNewButtonEnabled
                    ? this.VirtualView.SelectedTextColor.ToPlatform()
                    : this.VirtualView.DisabledTextColor.ToPlatform();

                var selectedTintColor = isNewButtonEnabled
                    ? this.VirtualView.TintColor.ToPlatform()
                    : this.VirtualView.DisabledTintColor.ToPlatform();

                rb.SetTextColor(selectedTextColor);
                SetTintColor(rb, selectedTintColor);

                //reset old selected button properties
                if (this.selectedRadioButton != null && this.selectedRadioButton.Id != rb.Id)
                {
                    var isOldButtonEnabled = this.selectedRadioButton.Enabled;
                    var textColor = isOldButtonEnabled
                        ? this.VirtualView.TextColor.ToPlatform()
                        : this.VirtualView.DisabledTextColor.ToPlatform();

                    var tintColor = isOldButtonEnabled
                        ? this.VirtualView.TintColor.ToPlatform()
                        : this.VirtualView.DisabledBackgroundColor.ToPlatform();

                    this.selectedRadioButton.SetTextColor(textColor);
                    SetTintColor(this.selectedRadioButton, tintColor);
                }

                this.selectedRadioButton = rb;

                this.VirtualView.SelectedSegment = radioId;
            }
        }

        private void ConfigureRadioButton(int i, bool isEnabled, RadioButton radioButton)
        {
            var isButtonEnabled = this.VirtualView.IsEnabled && isEnabled;

            if (radioButton.Enabled != isButtonEnabled)
            {
                radioButton.Enabled = isButtonEnabled;
            }

            var isSelected = i == this.VirtualView.SelectedSegment;

            var tintColor = this.GetTintColor(isSelected, isButtonEnabled);

            if (i == this.VirtualView.SelectedSegment)
            {
                var textColor = isButtonEnabled
                    ? this.VirtualView.SelectedTextColor.ToPlatform()
                    : this.VirtualView.DisabledTextColor.ToPlatform();

                radioButton.SetTextColor(textColor);
                this.selectedRadioButton = radioButton;
            }
            else
            {
                var textColor = isButtonEnabled
                    ? this.VirtualView.TextColor.ToPlatform()
                    : this.VirtualView.DisabledTextColor.ToPlatform();

                radioButton.SetTextColor(textColor);
            }

            if (this.VirtualView.FontSize is var fontSize && fontSize > 0d)
            {
                radioButton.SetTextSize(ComplexUnitType.Sp, (float)fontSize);
            }

            var typefaceResolver = this.GetRequiredService<ITypefaceResolver>();
            var typeface = typefaceResolver.GetTypeface(
                this.VirtualView.FontFamily,
                this.VirtualView.FontSize,
                this.VirtualView.FontAttributes);

            radioButton.SetTypeface(typeface, TypefaceStyle.Normal);


            SetTintColor(radioButton, tintColor);

            radioButton.Tag = i;
            radioButton.Click += this.RadioButton_Click;
        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                var t = (int)radioButton.Tag;
                this.VirtualView.RaiseSelectionChanged(t);
            }
        }

        private Android.Graphics.Color GetTintColor(bool selected, bool enabled)
        {
            return enabled
                ? this.VirtualView.TintColor.ToPlatform()
                : this.VirtualView.DisabledTintColor.ToPlatform();

            // 'tint' is an outline + selected button color, so
            // the background color for the segmented control can't be used as 'tint'

            // TODO we should have a separate outline color
            // and ability to pick a background color for selected(checked) segment
        }

        private static void SetTintColor(RadioButton radioButton, Android.Graphics.Color tintColor)
        {
            // Do not call SetBackgroundColor, that sets the state to ColorDrawable and makes an invalid cast
            var gradientDrawable = (StateListDrawable)radioButton.Background;
            var drawableContainerState = (DrawableContainer.DrawableContainerState)gradientDrawable.GetConstantState();
            var children = drawableContainerState.GetChildren();

            // Doesnt works on API < 18
            var selectedShape = children[0] as GradientDrawable ?? (GradientDrawable)((InsetDrawable)children[0]).Drawable;
            var unselectedShape = children[1] as GradientDrawable ?? (GradientDrawable)((InsetDrawable)children[1]).Drawable;

            selectedShape.SetStroke(3, tintColor);
            selectedShape.SetColor(tintColor);
            unselectedShape.SetStroke(3, tintColor);
        }

        private static void MapTintColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            OnPropertyChanged(handler, control);
        }
        
        private static void MapFontFamily(SegmentedControlHandler handler, SegmentedControl control)
        {
            OnPropertyChanged(handler, control);
        }
        
        private static void MapFontSize(SegmentedControlHandler handler, SegmentedControl control)
        {
            OnPropertyChanged(handler, control);
        }
        
        private static void MapFontAttributes(SegmentedControlHandler handler, SegmentedControl control)
        {
            OnPropertyChanged(handler, control);
        }

        private static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl control)
        {
            var option = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);

            if (option != null)
            {
                option.Checked = true;
            }

            var t = (int)option.Tag;
            control.RaiseSelectionChanged(t);
        }


        private static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl control)
        {
            OnPropertyChanged(handler, control);
        }

        private static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (control.SelectedSegment is not (var selectedSegment and >= 0))
            {
                return;
            }

            var radioButton = (RadioButton)handler.PlatformView.GetChildAt(selectedSegment);
            if (radioButton != null)
            {
                var textColor = control.SelectedTextColor.ToPlatform();
                radioButton.SetTextColor(textColor);
            }
        }

        private static void MapDisabledSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (control.SelectedSegment is not (var selectedSegment and >= 0))
            {
                return;
            }

            var radioButton = (RadioButton)handler.PlatformView.GetChildAt(selectedSegment);
            if (radioButton != null)
            {
                var textColor = control.DisabledSelectedTextColor.ToPlatform();
                radioButton.SetTextColor(textColor);
            }
        }

        private static void MapDisabledTintColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (control.SelectedSegment is not (var selectedSegment and >= 0))
            {
                return;
            }

            var radioButton = (RadioButton)handler.PlatformView.GetChildAt(selectedSegment);
            if (radioButton != null)
            {
                var isButtonEnabled = control.IsEnabled && radioButton.Enabled;
                var tintColor = handler.GetTintColor(true, isButtonEnabled);
                SetTintColor(radioButton, tintColor);
            }
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
                    if (radioButton == null)
                    {
                        continue;
                    }

                    if (!radioButton.Enabled || !control.IsEnabled)
                    {
                        radioButton.SetTextColor(disabledTextColor);
                    }
                }
            }
        }

        private static void MapDisabledBackgroundColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            //go through children and update disabled segments
            for (var i = 0; i < handler.PlatformView.ChildCount; i++)
            {
                var radioButton = (RadioButton)handler.PlatformView.GetChildAt(i);
                if (radioButton == null)
                {
                    continue;
                }

                var tintColor = handler.GetTintColor(radioButton.Checked, !radioButton.Enabled || !control.IsEnabled);
                SetTintColor(radioButton, tintColor);
            }
        }

        private static void MapChildren(SegmentedControlHandler handler, SegmentedControl control)
        {
            // If the entire Children property has been changed,
            // we have to recreate all the segments from scratch.

            if (handler.PlatformView is RadioGroup radioGroup && control != null)
            {
                var count = radioGroup.ChildCount;
                if (count > 0)
                {
                    // Remove existing items from radio group
                    for (var i = count - 1; i >= 0; i--)
                    {
                        var o = radioGroup.GetChildAt(i);
                        if (o is RadioButton)
                        {
                            radioGroup.RemoveViewAt(i);
                        }
                    }

                    // Next, add new children
                    var layoutInflater = LayoutInflater.From(handler.Context);

                    var segmentedControl = handler.VirtualView;
                    for (var i = 0; i < segmentedControl.Children.Count; i++)
                    {
                        var o = segmentedControl.Children[i];
                        var isButtonEnabled = segmentedControl.IsEnabled && o.IsEnabled;
                        var rb = (RadioButton)layoutInflater.Inflate(Resource.Layout.RadioButton, null);

                        rb.LayoutParameters = new RadioGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent, 1f);
                        rb.Text = o.Text;

                        if (i == 0)
                        {
                            rb.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                        }
                        else if (i == segmentedControl.Children.Count - 1)
                        {
                            rb.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                        }

                        handler.ConfigureRadioButton(i, isButtonEnabled, rb);
                        radioGroup.AddView(rb);
                    }

                    var option = (RadioButton)radioGroup.GetChildAt(segmentedControl.SelectedSegment);
                    if (option != null)
                    {
                        option.Checked = true;
                    }
                }
            }
        }

        private static void MapTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            var childCount = handler.PlatformView.ChildCount;
            if (childCount > 0)
            {
                var textColor = control.TextColor.ToPlatform();
                var selectedTextColor = control.SelectedTextColor.ToPlatform();

                for (var i = 0; i < childCount; i++)
                {
                    var radioButton = (RadioButton)handler.PlatformView.GetChildAt(i);
                    if (radioButton == null)
                    {
                        continue;
                    }

                    if (i != control.SelectedSegment)
                    {
                        radioButton.SetTextColor(textColor);
                    }
                    else
                    {
                        radioButton.SetTextColor(selectedTextColor);
                    }
                }
            }

        }

        private static void OnPropertyChanged(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (handler.PlatformView is RadioGroup radioGroup && control != null)
            {
                for (var i = 0; i < control.Children.Count; i++)
                {
                    var radioButton = (RadioButton)radioGroup.GetChildAt(i);
                    if (radioButton == null)
                    {
                        continue;
                    }

                    var segmentedControlOption = control.Children[i];

                    if (radioButton.Text != segmentedControlOption.Text)
                    {
                        radioButton.Text = segmentedControlOption.Text;
                    }

                    handler.ConfigureRadioButton(i, control.Children[i].IsEnabled, radioButton);
                }
            }
        }
    }
}