using System.Reflection;
using Microsoft.Extensions.Logging;

namespace SegmentedControlDemoApp.Services
{
    public class MauiNavigationService : INavigationService
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public MauiNavigationService(
            ILogger<MauiNavigationService> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task PushAsync(string pageName, bool animated = true)
        {
            try
            {
                var page = this.ResolvePage(pageName);
                var navigation = GetNavigation();
                await navigation.PushAsync(page, animated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PushAsync failed with exception");
                throw;
            }
        }

        public async Task PushModalAsync(string pageName, bool animated = true)
        {
            try
            {
                var page = this.ResolvePage(pageName);
                var navigation = GetNavigation();
                await navigation.PushModalAsync(new NavigationPage(page), animated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PushModalAsync failed with exception");
                throw;
            }
        }

        private Page ResolvePage(string pageName)
        {
            var pageTypes = FindTypesWithName(pageName);
            if (pageTypes.Length == 0)
            {
                throw new PageNavigationException($"Page with name '{pageName}' not found");
            }

            if (pageTypes.Length > 1)
            {
                throw new PageNavigationException(
                    $"Multiple pages found for name '{pageName}': " +
                    $"{string.Join($"> {Environment.NewLine}", pageTypes.Select(t => t.FullName))}");
            }

            var pageType = pageTypes.Single();
            var page = (Page)this.serviceProvider.GetRequiredService(pageType);

            var viewModelName = pageName.Substring(0, pageName.LastIndexOf("Page")) + "ViewModel";
            var viewModelTypes = FindTypesWithName(viewModelName);

            if (viewModelTypes.Length == 0)
            {
                this.logger.LogInformation($"View model with name '{viewModelName}' not found");
            }
            else if (viewModelTypes.Length == 1)
            {
                var viewModelType = viewModelTypes.Single();
                var viewModel = this.serviceProvider.GetRequiredService(viewModelType);
                page.BindingContext = viewModel;
            }
            else
            {
                throw new PageNavigationException(
                    $"Multiple view models found for name '{viewModelName}': " +
                    $"{string.Join($"> {Environment.NewLine}", viewModelTypes.Select(t => t.FullName))}");
            }

            return page;
        }

        private static INavigation GetNavigation()
        {
            if (Shell.Current != null)
            {
                throw new NotSupportedException(
                    $"{nameof(MauiNavigationService)} does currently not support AppShell navigation");
            }

            if (Application.Current?.MainPage is not Page page)
            {
                throw new PageNavigationException("Application.Current.MainPage is not set");
            }

            var targetPage = GetTarget(page);
            var navigation = targetPage.Navigation;

            if (navigation.ModalStack.Count > 0)
            {
                var modalPage = GetTarget(navigation.ModalStack.Last());
                var modalNavigation = modalPage.Navigation;
                return modalNavigation;
            }

            return navigation;
        }

        private static Page GetTarget(Page target)
        {
            return target switch
            {
                FlyoutPage flyout => GetTarget(flyout.Detail),
                TabbedPage tabbed => GetTarget(tabbed.CurrentPage),
                NavigationPage navigation => GetTarget(navigation.CurrentPage) ?? navigation,
                ContentPage page => page,
                null => null,
                _ => throw new NotSupportedException($"The page type '{target.GetType().FullName}' is not supported.")
            };
        }

        private static Type[] FindTypesWithName(string typeName)
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => string.Equals(t.Name, typeName, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public async Task PopAsync(bool animated = true)
        {
            try
            {
                var navigation = GetNavigation();
                await navigation.PopAsync(animated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PopAsync failed with exception");
                throw;
            }
        }

        public async Task PopToRootAsync(bool recursive = true, bool acrossModals = false, bool animated = true)
        {
            try
            {
                var navigation = GetNavigation();

                var hasModalStack = navigation.ModalStack.Any();

                if (recursive)
                {
                    foreach (var page in navigation.NavigationStack.ToArray().Skip(hasModalStack ? 0 : 1).Reverse())
                    {
                        if (hasModalStack && navigation.NavigationStack.FirstOrDefault() == page)
                        {
                            await page.Navigation.PopModalAsync(animated);
                        }
                        else
                        {
                            await page.Navigation.PopAsync(animated);
                        }
                    }
                }
                else
                {
                    await navigation.PopToRootAsync(animated);
                }

                if (acrossModals && hasModalStack)
                {
                    if (!recursive)
                    {
                        await this.PopModalAsync(animated);
                    }

                    await this.PopToRootAsync(recursive, acrossModals, animated);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PopToRootAsync failed with exception");
                throw;
            }
        }

        public async Task PopModalAsync(bool animated = true)
        {
            try
            {
                var navigation = GetNavigation();
                await navigation.PopModalAsync(animated);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PopModalAsync failed with exception");
                throw;
            }
        }

        public async Task NavigateAsync(string pageName, bool animated = true)
        {
            try
            {
                var path = pageName.Trim();
                var isAbsolute = path.StartsWith('/');
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var pages = this.ResolvePagesForSegments(segments.First(), segments.Skip(1)).ToArray();

                var navigation = GetNavigation();

                if (isAbsolute)
                {
                    var rootPage = pages.First();
                    Application.Current.MainPage = rootPage;
                }

                foreach (var page in pages.Skip(isAbsolute ? 1 : 0))
                {
                    await navigation.PushAsync(page, animated);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "PopModalAsync failed with exception");
                throw;
            }
        }

        private IEnumerable<Page> ResolvePagesForSegments(string firstSegment, IEnumerable<string> segments)
        {
            if (firstSegment == nameof(NavigationPage))
            {
                if (segments.Any())
                {
                    var pages = this.ResolvePagesForSegments(segments.First(), segments.Skip(1));
                    var firstPage = pages.First();
                    yield return new NavigationPage(firstPage);
                    foreach (var childPage in pages.Skip(1))
                    {
                        yield return childPage;
                    }
                }
                else
                {
                    yield return new NavigationPage();
                }

                yield break;
            }

            var page = this.ResolvePage(firstSegment);
            yield return page;

            {
                if (segments.Any())
                {
                    var pages = this.ResolvePagesForSegments(segments.First(), segments.Skip(1));
                    foreach (var childPage in pages)
                    {
                        yield return childPage;
                    }
                }
            }
        }
    }

    public class PageNavigationException : Exception
    {
        public PageNavigationException(string message) : base(message)
        {
        }
    }
}