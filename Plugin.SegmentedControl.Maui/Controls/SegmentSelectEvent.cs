namespace Plugin.SegmentedControl.Maui
{
    [Preserve(AllMembers = true)]
    public class SelectedIndexChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
}
