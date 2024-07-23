using Microsoft.Maui.Handlers;

namespace Plugin.SegmentedControl.Maui.Extensions
{
    internal static class ElementHandlerExtensions
	{
		private static IServiceProvider GetServiceProvider(this IElementHandler handler)
		{
			var context = handler.MauiContext ??
				throw new InvalidOperationException($"Unable to find the context. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");

			var services = context?.Services ??
				throw new InvalidOperationException($"Unable to find the service provider. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");

			return services;
		}

		public static T GetRequiredService<T>(this IElementHandler handler)
			where T : notnull
		{
			var services = handler.GetServiceProvider();

			var service = services.GetRequiredService<T>();

			return service;
		}
	}
}