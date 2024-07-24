namespace Plugin.SegmentedControl.Maui
{
    [Preserve(AllMembers = true)]
    public class ChildrenChangingEventArgs : EventArgs
    {
        public ChildrenChangingEventArgs(IList<SegmentedControlOption> oldValues, IList<SegmentedControlOption> newValues)
        {
            this.OldValues = oldValues;
            this.NewValues = newValues;
        }

        public IList<SegmentedControlOption> OldValues { get; }

        public IList<SegmentedControlOption> NewValues { get; }
    }
}
