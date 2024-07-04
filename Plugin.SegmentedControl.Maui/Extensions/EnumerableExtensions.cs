namespace Plugin.SegmentedControl.Maui
{
    internal static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var i = 0;
            foreach (T element in enumerable)
            {
                if (predicate(element))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
