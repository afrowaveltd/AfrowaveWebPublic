namespace Id.Tests.Helpers
{
	/// <summary>
	/// Provides a helper to create Razor PageModel instances with preconfigured PageContext for testing.
	/// </summary>
	public static class PageFactory
	{
		/// <summary>
		/// Creates a new instance of the specified PageModel type and sets up the required context for testing.
		/// </summary>
		/// <typeparam name="T">Type of the PageModel</typeparam>
		/// <param name="services">The service provider used to resolve dependencies</param>
		/// <returns>Configured instance of the PageModel</returns>
		public static T Create<T>(IServiceProvider services) where T : PageModel
		{
			DefaultHttpContext httpContext = new DefaultHttpContext()
			{
				RequestServices = services
			};

			ModelStateDictionary modelState = new ModelStateDictionary();
			ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
			ViewDataDictionary viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
			TempDataDictionary tempData = new TempDataDictionary(httpContext, services.GetRequiredService<ITempDataProvider>());

			PageContext pageContext = new PageContext(actionContext)
			{
				ViewData = viewData
			};

			T page = services.GetRequiredService<T>();
			page.PageContext = pageContext;
			page.TempData = tempData;

			return page;
		}
	}
}