using Microsoft.Extensions.DependencyInjection;
using SharedTools.Services;

namespace SharedTools.Extensions;

/// <summary>
/// Extension methods for the EmojiService.
/// </summary>
public static class EmojiServiceExtensions
{
	/// <summary>
	/// Registers the EmojiService and its interface into the DI container.
	/// </summary>
	/// <param name="services">The IServiceCollection to add the service to.</param>
	/// <returns>The updated IServiceCollection.</returns>
	public static IServiceCollection AddEmojiService(this IServiceCollection services)
	{
		_ = services.AddSingleton<IEmojiService, EmojiService>();
		return services;
	}

	/// <summary>
	/// Returns the emoji C# string based on input name or UTF code.
	/// </summary>
	public static string ToEmoji(this string input, IEmojiService service)
	{
		return service.Get(input);
	}
}