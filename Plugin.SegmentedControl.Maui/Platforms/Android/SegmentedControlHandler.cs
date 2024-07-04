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
        private RadioButton radioButton;

        public static IPropertyMapper<SegmentedControl, SegmentedControlHandler> Mapper = new PropertyMapper<SegmentedControl, SegmentedControlHandler>(ViewMapper)
        {
            [nameof(SegmentedControl.IsEnabled)] = MapIsEnabled,
            [nameof(SegmentedControl.SelectedSegment)] = MapSelectedSegment,
            [nameof(SegmentedControl.TintColor)] = MapTintColor,
            [nameof(SegmentedControl.SelectedTextColor)] = MapSelectedTextColor,
            [nameof(SegmentedControl.TextColor)] = MapTextColor
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
                var segmentedControlOption = this.VirtualView.Children[i];
                var radioButton = (RadioButton)layoutInflater.Inflate(Resource.Layout.RadioButton, null);

                radioButton.LayoutParameters = new RadioGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent, 1f);
                radioButton.Text = segmentedControlOption.Text;

                if (i == 0)
                {
                    radioButton.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                }
                else if (i == this.VirtualView.Children.Count - 1)
                {
                    radioButton.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
                }

                this.ConfigureRadioButton(i, radioButton);

                nativeControl.AddView(radioButton);
            }

            {
                var radioButton = (RadioButton)nativeControl.GetChildAt(this.VirtualView.SelectedSegment);

                if (radioButton != null)
                {
                    radioButton.Checked = true;
                }
            }

            return nativeControl;
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
            this.radioButton = null;
        }

        private void PlatformView_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var radioGroup = (RadioGroup)sender;
            if (radioGroup.CheckedRadioButtonId != -1)
            {
                var id = radioGroup.CheckedRadioButtonId;
                var radioButtonView = radioGroup.FindViewById(id);
                var radioId = radioGroup.IndexOfChild(radioButtonView);

                var radioButton = (RadioButton)radioGroup.GetChildAt(radioId);

                var textColor = this.VirtualView.IsEnabled
                    ? this.VirtualView.TextColor.ToPlatform()
                    : this.VirtualView.DisabledColor.ToPlatform();

                this.radioButton?.SetTextColor(textColor);

                radioButton.SetTextColor(this.VirtualView.SelectedTextColor.ToPlatform());
                this.radioButton = radioButton;

                this.VirtualView.SelectedSegment = radioId;
            }
        }

        private void ConfigureRadioButton(int i, RadioButton rb)
        {
            if (i == this.VirtualView.SelectedSegment)
            {
                rb.SetTextColor(this.VirtualView.SelectedTextColor.ToPlatform());
                this.radioButton = rb;
            }
            else
            {
                var textColor = this.VirtualView.IsEnabled ? this.VirtualView.TintColor.ToPlatform() : this.VirtualView.DisabledColor.ToPlatform();
                rb.SetTextColor(textColor);
            }

            GradientDrawable selectedShape;
            GradientDrawable unselectedShape;

            var gradientDrawable = (StateListDrawable)rb.Background;
            var drawableContainerState = (DrawableContainer.DrawableContainerState)gradientDrawable.GetConstantState();
            var children = drawableContainerState.GetChildren();

            // Doesnt works on API < 18
            selectedShape = children[0] is GradientDrawable drawable ? drawable : (GradientDrawable)((InsetDrawable)children[0]).Drawable;
            unselectedShape = children[1] is GradientDrawable drawable1 ? drawable1 : (GradientDrawable)((InsetDrawable)children[1]).Drawable;

            var color = this.VirtualView.IsEnabled ? this.VirtualView.TintColor.ToPlatform() : this.VirtualView.DisabledColor.ToPlatform();

            selectedShape.SetStroke(3, color);
            selectedShape.SetColor(color);
            unselectedShape.SetStroke(3, color);

            rb.Enabled = this.VirtualView.IsEnabled;
        }

        private static void MapTintColor(SegmentedControlHandler handler, SegmentedControl control) => OnPropertyChanged(handler, control);

        private static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl control)
        {
            var option = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);

            if (option != null)
            {
                option.Checked = true;
            }

            control.RaiseSelectionChanged();
        }

        private static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl control) => OnPropertyChanged(handler, control);

        private static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            var v = (RadioButton)handler.PlatformView.GetChildAt(control.SelectedSegment);
            v?.SetTextColor(control.SelectedTextColor.ToPlatform());
        }

        private static void MapTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            for (int i = 0; i < handler.PlatformView.ChildCount; i++)
            {
                var v = (RadioButton)handler.PlatformView.GetChildAt(i);
                if (i != control.SelectedSegment)
                {
                    v.SetTextColor(control.TextColor.ToPlatform());
                }
            }
        }

        private static void OnPropertyChanged(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (handler.PlatformView != null && control != null)
            {
                for (var i = 0; i < control.Children.Count; i++)
                {
                    var rb = (RadioButton)handler.PlatformView.GetChildAt(i);

                    handler.ConfigureRadioButton(i, rb);
                }
            }
        }
    }
}
