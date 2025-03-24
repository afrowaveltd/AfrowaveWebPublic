namespace Id.Tests.Helpers
{
	/// <summary>
	/// Provides a default service registration for Razor PageModel testing in Afrowave.
	/// Includes logging, localization, and TempData services.
	/// </summary>
	public static class AfrowaveTestServices
	{
		/// <summary>
		/// Builds a service provider with default testing services and optional overrides.
		/// </summary>
		/// <typeparam name="TPageModel">Type of the PageModel being tested</typeparam>
		/// <param name="overrides">Optional lambda for registering custom or mock services</param>
		/// <returns>Service provider configured for unit testing</returns>
		public static IServiceProvider BuildWithDefaults<TPageModel>(Action<IServiceCollection>? overrides = null)
			where TPageModel : class
		{
			ServiceCollection services = new ServiceCollection();

			_ = services.AddSingleton(Substitute.For<ILogger<TPageModel>>());
			_ = services.AddSingleton(Substitute.For<IStringLocalizer<TPageModel>>());
			_ = services.AddSingleton<ITempDataProvider, FakeTempDataProvider>();
			_ = services.AddSingleton<TPageModel>();

			overrides?.Invoke(services);

			return services.BuildServiceProvider();
		}
	}
}