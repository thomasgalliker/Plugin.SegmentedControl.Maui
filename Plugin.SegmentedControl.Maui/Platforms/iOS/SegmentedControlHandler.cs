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

            UpdateIsEnabled(uiSegmentedControl, segmentedControl);

            uiSegmentedControl.TintColor = segmentedControl.IsEnabled
                ? segmentedControl.TintColor.ToPlatform()
                : segmentedControl.DisabledTintColor.ToPlatform();

            UpdateTitleTextAttributesNormal(uiSegmentedControl, segmentedControl);
            UpdateTitleTextAttributesSelected(uiSegmentedControl, segmentedControl);

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

        private static void MapTintColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTintColor(handler.PlatformView, segmentedControl);
        }

        private static void MapDisabledTintColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTintColor(handler.PlatformView, segmentedControl);
        }

        private static void UpdateTintColor(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                uiSegmentedControl.SelectedSegmentTintColor = segmentedControl.IsEnabled
                    ? segmentedControl.TintColor.ToPlatform()
                    : segmentedControl.DisabledTintColor.ToPlatform();
            }
            else
            {
                uiSegmentedControl.TintColor = segmentedControl.IsEnabled
                    ? segmentedControl.TintColor.ToPlatform()
                    : segmentedControl.DisabledTintColor.ToPlatform();
            }
        }

        private static void MapSelectedSegment(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            var uiSegmentedControl = handler.PlatformView;
            uiSegmentedControl.SelectedSegment = segmentedControl.SelectedSegment;
            segmentedControl.RaiseSelectionChanged(segmentedControl.SelectedSegment);
        }

        private static void MapIsEnabled(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateIsEnabled(handler.PlatformView, segmentedControl);
        }

        private static void UpdateIsEnabled(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            uiSegmentedControl.Enabled = segmentedControl.IsEnabled;

            UpdateTitleTextAttributesNormal(uiSegmentedControl, segmentedControl);
            UpdateTitleTextAttributesSelected(uiSegmentedControl, segmentedControl);
            UpdateTintColor(uiSegmentedControl, segmentedControl);
        }

        private static void MapSelectedTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesSelected(handler.PlatformView, segmentedControl);
        }

        private static void MapDisabledSelectedTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesSelected(handler.PlatformView, segmentedControl);
        }

        private static void MapTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesNormal(handler.PlatformView, segmentedControl);
        }

        private static void MapDisabledTextColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesNormal(handler.PlatformView, segmentedControl);
        }

        private static void MapFontAttributes(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesNormal(handler.PlatformView, segmentedControl);
        }

        private static void MapFontSize(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesNormal(handler.PlatformView, segmentedControl);
        }

        private static void MapFontFamily(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            UpdateTitleTextAttributesNormal(handler.PlatformView, segmentedControl);
        }

        private static void UpdateTitleTextAttributesNormal(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            var textColor = segmentedControl.IsEnabled ? segmentedControl.TextColor : segmentedControl.DisabledTextColor;
            UpdateTitleTextAttributes(uiSegmentedControl, segmentedControl, textColor, UIControlState.Normal);
        }

        private static void UpdateTitleTextAttributesSelected(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl)
        {
            var textColor = segmentedControl.IsEnabled ? segmentedControl.SelectedTextColor : segmentedControl.DisabledSelectedTextColor;
            UpdateTitleTextAttributes(uiSegmentedControl, segmentedControl, textColor, UIControlState.Selected);
        }

        private static void UpdateTitleTextAttributes(UISegmentedControl uiSegmentedControl, SegmentedControl segmentedControl,
            Color textColor, UIControlState controlState)
        {
            var font = FontHelper.CreateFont(
                segmentedControl.FontFamily,
                segmentedControl.FontSize,
                segmentedControl.FontAttributes);

            var fontManager = segmentedControl.Handler.GetRequiredService<IFontManager>();
            var uiFont = fontManager.GetFont(font);

            uiSegmentedControl.SetTitleTextAttributes(
                new UIStringAttributes { Font = uiFont, ForegroundColor = textColor.ToPlatform(), }, controlState);
        }

        private static void MapDisabledBackgroundColor(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }

        private static void MapChildren(SegmentedControlHandler handler, SegmentedControl segmentedControl)
        {
            // TODO: Implement
        }
    }
}