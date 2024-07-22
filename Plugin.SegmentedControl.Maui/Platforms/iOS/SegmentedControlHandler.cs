using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Plugin.SegmentedControl.Maui.Extensions;
using Plugin.SegmentedControl.Maui.Utils;
using UIKit;
using Font = Microsoft.Maui.Font;

namespace Plugin.SegmentedControl.Maui
{
    public class SegmentedControlHandler : ViewHandler<SegmentedControl, UISegmentedControl>
    {
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

        public SegmentedControlHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {
        }

        protected override UISegmentedControl CreatePlatformView()
        {
            var uiSegmentedControl = new UISegmentedControl();
            var segmentedControl = this.VirtualView;

            // uiSegmentedControl.RemoveAllSegments();

            for (var i = 0; i < segmentedControl.Children.Count; i++)
            {
                uiSegmentedControl.InsertSegment(segmentedControl.Children[i].Text, i, false);
            }

            // TODO: Deduplicate assignments

            uiSegmentedControl.Enabled = segmentedControl.IsEnabled;

            uiSegmentedControl.TintColor = segmentedControl.IsEnabled
                ? segmentedControl.TintColor.ToPlatform()
                : segmentedControl.DisabledTintColor.ToPlatform();

            UpdateSelectedTextColor(uiSegmentedControl, segmentedControl);
            uiSegmentedControl.SelectedSegment = segmentedControl.SelectedSegment;
            return uiSegmentedControl;
        }

        protected override void ConnectHandler(UISegmentedControl platformView)
        {
            base.ConnectHandler(platformView);

            if (this.VirtualView.AutoDisconnectHandler)
            {
                this.VirtualView.AddCleanUpEvent();
            }

            platformView.ValueChanged += this.UISegmentedControl_ValueChanged;
        }

        protected override void DisconnectHandler(UISegmentedControl platformView)
        {
            platformView.ValueChanged -= this.UISegmentedControl_ValueChanged;

            base.DisconnectHandler(platformView);
        }

        private void UISegmentedControl_ValueChanged(object sender, EventArgs e)
        {
            var uiSegmentedControl = this.PlatformView;
            var segmentedControl = this.VirtualView;
            segmentedControl.SelectedSegment = (int)uiSegmentedControl.SelectedSegment;
        }

        private static void MapTintColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                handler.PlatformView.SelectedSegmentTintColor = control.IsEnabled
                    ? control.TintColor.ToPlatform()
                    : control.DisabledTintColor.ToPlatform();
            }
            else
            {
                handler.PlatformView.TintColor = control.IsEnabled
                    ? control.TintColor.ToPlatform()
                    : control.DisabledTintColor.ToPlatform();
            }
        }

        private static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl control)
        {
            handler.PlatformView.SelectedSegment = control.SelectedSegment;
            control.RaiseSelectionChanged(control.SelectedSegment);
        }

        private static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            var uiSegmentedControl = handler.PlatformView;

            uiSegmentedControl.Enabled = segmentedControl.IsEnabled;

            // MapTintColor(handler, control);

            uiSegmentedControl.TintColor = segmentedControl.IsEnabled
                ? segmentedControl.TintColor.ToPlatform()
                : segmentedControl.DisabledTintColor.ToPlatform();
        }

        private static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            UpdateSelectedTextColor(handler.PlatformView, control);
        }

        private static void UpdateSelectedTextColor(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            try
            {
                var selectedTextColor = segmentedControl.SelectedTextColor.ToPlatform();

                // UIStringAttributes uiTextAttribute = uiSegmentedControl.GetTitleTextAttributes(UIControlState.Normal);
                // uiTextAttribute.ForegroundColor = selectedTextColor;
                // uiSegmentedControl.SetTitleTextAttributes(uiTextAttribute, UIControlState.Selected);

                uiSegmentedControl.SetTitleTextAttributes(
                    new UIStringAttributes { ForegroundColor = selectedTextColor }, UIControlState.Selected);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void MapTextColor(SegmentedControlHandler handler, SegmentedControl control)
        {
            UpdateTextColor(handler.PlatformView, control);
        }

        private static void UpdateTextColor(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            var textColor = segmentedControl.SelectedTextColor.ToPlatform();
            uiSegmentedControl.SetTitleTextAttributes(
                new UIStringAttributes { ForegroundColor = textColor }, UIControlState.Normal);
        }

        private static void MapChildren(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }

        private static void MapFontAttributes(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateFonts(handler.PlatformView, segmentedControl);
        }

        private static void MapFontSize(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateFonts(handler.PlatformView, segmentedControl);
        }

        private static void MapFontFamily(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateFonts(handler.PlatformView, segmentedControl);
        }

        private static void UpdateFonts(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            // var uiTextAttribute = uiSegmentedControl.GetTitleTextAttributes(UIControlState.Normal);

            var font = FontHelper.CreateFont(
                segmentedControl.FontFamily,
                segmentedControl.FontSize,
                segmentedControl.FontAttributes);

            var fontManager = segmentedControl.Handler.GetRequiredService<IFontManager>();
            var uiFont = fontManager.GetFont(font);

            uiSegmentedControl.SetTitleTextAttributes(
                new UIStringAttributes { Font = uiFont }, UIControlState.Normal);
        }

        private static void MapDisabledBackgroundColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }

        private static void MapDisabledSelectedTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }

        private static void MapDisabledTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }

        private static void MapDisabledTintColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }
    }
}