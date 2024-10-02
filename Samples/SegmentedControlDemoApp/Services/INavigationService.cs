namespace SegmentedControlDemoApp.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// Pushes the given <paramref name="page"/> to the navigation stack.
        /// </summary>
        Task PushAsync(string page, bool animated = true);

        /// <summary>
        /// Pushes the given <paramref name="page"/> to the navigation stack in a modal context.
        /// </summary>
        Task PushModalAsync(string page, bool animated = true);

        /// <summary>
        /// Pops back from the current page.
        /// </summary>
        Task PopAsync(bool animated = true);

        /// <summary>
        /// Pops back to the root page of the navigation stack.
        /// </summary>
        /// <param name="recursive">
        /// Pops back one page after the other, if <c>true</c>.
        /// Removes all pages at once, if <c>false</c>.
        /// </param>
        /// <param name="acrossModals">Pops non-modal pages as well as modal pages, if <c>true</c>.</param>
        /// <param name="animated">Animates the navigation calls.</param>
        Task PopToRootAsync(bool recursive = true, bool acrossModals = false, bool animated = true);

        /// <summary>
        /// Pops a modal page from the navigation stack.
        /// </summary>
        Task PopModalAsync(bool animated = true);

        /// <summary>
        /// Replaces the current root page with <paramref name="page"/>.
        /// </summary>
        Task NavigateAsync(string page, bool animated = true);
    }
}