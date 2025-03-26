using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Id.Tests.Helpers
{
	/// <summary>
	/// Base class for unit testing Razor Pages using shared PageFactory and default service provider.
	/// </summary>
	/// <typeparam name="TPageModel">The Razor PageModel type under test</typeparam>
	public abstract class RazorPageTestBase<TPageModel> where TPageModel : PageModel
	{
		/// <summary>
		/// The service provider used to resolve dependencies during test setup.
		/// </summary>
		protected IServiceProvider Services;

		/// <summary>
		/// Initializes a new instance of the test base with default services for the PageModel type.
		/// </summary>
		protected RazorPageTestBase()
		{
			Services = AfrowaveTestServices.BuildWithDefaults<TPageModel>(ConfigureServices);
		}

		/// <summary>
		/// Allows overriding or extending registered services for the test.
		/// </summary>
		/// <param name="services">The service collection to modify</param>
		protected virtual void ConfigureServices(IServiceCollection services)
		{
			// Child test classes can override and add mock services here
		}

		/// <summary>
		/// Creates and configures a PageModel instance with PageContext and TempData.
		/// </summary>
		/// <returns>The testable instance of the PageModel</returns>
		protected TPageModel CreatePageModel() => PageFactory.Create<TPageModel>(Services);

		/// <summary>
		/// Overrides a service registration with a new instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		protected void OverrideService<T>(T instance) where T : class
		{
			ServiceCollection services = new ServiceCollection();
			_ = services.AddSingleton(instance);

			// Zachová ostatní služby
			foreach(ServiceDescriptor descriptor in ((ServiceProvider)Services).GetService<IServiceCollection>() ?? new ServiceCollection())
			{
				if(descriptor.ServiceType != typeof(T))
				{
					_ = services.Add(descriptor);
				}
			}

			Services = services.BuildServiceProvider();
		}
	}
}