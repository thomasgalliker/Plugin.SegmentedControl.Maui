using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Plugin.SegmentedControl.Maui
{
    public class SegmentedControlHandler : ViewHandler<SegmentedControl, UISegmentedControl>
    {
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

        protected override UISegmentedControl CreatePlatformView()
        {
            var segmentControl = new UISegmentedControl();
            for (var i = 0; i < this.VirtualView.Children.Count; i++)
            {
                segmentControl.InsertSegment(this.VirtualView.Children[i].Text, i, false);
            }

            // TODO: Deduplicate assignments

            segmentControl.Enabled = this.VirtualView.IsEnabled;

            segmentControl.TintColor = this.VirtualView.IsEnabled
                ? this.VirtualView.TintColor.ToPlatform()
                : this.VirtualView.DisabledColor.ToPlatform();

            segmentControl.SetTitleTextAttributes(new UIStringAttributes
            {
                ForegroundColor = this.VirtualView.SelectedTextColor.ToPlatform()
            }, UIControlState.Selected);

            segmentControl.SelectedSegment = this.VirtualView.SelectedSegment;
            return segmentControl;
        }

        protected override void ConnectHandler(UISegmentedControl platformView)
        {
            base.ConnectHandler(platformView);

            platformView.ValueChanged += this.PlatformView_ValueChanged;
        }

        protected override void DisconnectHandler(UISegmentedControl platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.ValueChanged -= this.PlatformView_ValueChanged;
        }

        void PlatformView_ValueChanged(object sender, EventArgs e)
        {
            this.VirtualView.SelectedSegment = (int)this.PlatformView.SelectedSegment;
        }

        private static void MapTintColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                handler.PlatformView.SelectedSegmentTintColor = control.IsEnabled
                    ? control.TintColor.ToPlatform()
                    : control.DisabledColor.ToPlatform();
            }
            else
            {
                handler.PlatformView.TintColor = control.IsEnabled
                    ? control.TintColor.ToPlatform()
                    : control.DisabledColor.ToPlatform();
            }
        }

        private static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl control)
        {
            handler.PlatformView.SelectedSegment = control.SelectedSegment;
            control.RaiseSelectionChanged();
        }

        private static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl control)
        {
            handler.PlatformView.Enabled = control.IsEnabled;

            handler.PlatformView.TintColor = control.IsEnabled
                ? control.TintColor.ToPlatform()
                : control.DisabledColor.ToPlatform();
        }

        private static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            handler.PlatformView.SetTitleTextAttributes(
                new UIStringAttributes()
                {
                    ForegroundColor = control.SelectedTextColor.ToPlatform()
                }, UIControlState.Selected);
        }

        private static void MapTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            handler.PlatformView.SetTitleTextAttributes(
                new UIStringAttributes()
                {
                    ForegroundColor = control.TextColor.ToPlatform()
                }, UIControlState.Normal);
        }

    }
}
