namespace Plugin.SegmentedControl.Maui
{
    [Preserve(AllMembers = true)]
    public class ElementChildrenChanging : EventArgs
    {
        public ElementChildrenChanging(IList<SegmentedControlOption> oldValues, IList<SegmentedControlOption> newValues)
        {
            this.OldValues = oldValues;
            this.NewValues = newValues;
        }

        public IList<SegmentedControlOption> OldValues { get; }

        public IList<SegmentedControlOption> NewValues { get; }
    }
}
