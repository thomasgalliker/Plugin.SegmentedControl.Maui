namespace SegmentedControlDemoApp.Utils
{
    public static class IdGenerator
    {
        private static int current;

        public static string GetNextId()
        {
            return Interlocked.Increment(ref current).ToString();
        }
    }
}