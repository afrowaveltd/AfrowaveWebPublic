namespace Id.Tests.Middlewares
{
	/// <summary>
	/// Unit tests for the <see cref="CustomErrorHandlingMiddleware"/> class using NSubstitute.
	/// </summary>
	public class CustomErrorHandlingMiddlewareTests
	{
		private readonly RequestDelegate _mockNext;
		private readonly ILogger<CustomErrorHandlingMiddleware> _mockLogger;
		private readonly IWebHostEnvironment _mockEnv;
		private readonly IStringLocalizer<CustomErrorHandlingMiddleware> _mockLocalizer;

		/// <summary>
		/// Sets up a test environment for the CustomErrorHandlingMiddleware. Mocks dependencies like RequestDelegate,
		/// ILogger, IWebHostEnvironment, and IStringLocalizer.
		/// </summary>
		public CustomErrorHandlingMiddlewareTests()
		{
			_mockNext = Substitute.For<RequestDelegate>();
			_mockLogger = Substitute.For<ILogger<CustomErrorHandlingMiddleware>>();
			_mockEnv = Substitute.For<IWebHostEnvironment>();
			_mockLocalizer = Substitute.For<IStringLocalizer<CustomErrorHandlingMiddleware>>();

			_mockEnv.EnvironmentName.Returns("Production");

			_mockLocalizer[Arg.Any<string>()].Returns(callInfo =>
			{
				var key = callInfo.Arg<string>();
				var value = key switch
				{
					"Error_404_Title" => new LocalizedString(key, "Not Found"),
					"Error_500_Title" => new LocalizedString(key, "Server Error"),
					"Error_Generic_Title" => new LocalizedString(key, "Error"),
					_ => new LocalizedString(key, key)
				};
				return value;
			});
		}

		private HttpContext CreateTestContext(string acceptHeader = "application/json")
		{
			var context = new DefaultHttpContext();
			context.Response.Body = new MemoryStream();
			context.Request.Headers["Accept"] = acceptHeader;

			var services = new ServiceCollection();
			services.AddSingleton(_mockLocalizer);
			context.RequestServices = services.BuildServiceProvider();

			return context;
		}

		/// <summary>
		/// Handles error responses for HTML requests by redirecting to an error page when a 404 status code is encountered. It
		/// sets the response status to 302 and updates the location header accordingly.
		/// </summary>
		/// <returns>No return value as it is an asynchronous task.</returns>
		[Fact]
		public async Task HandleErrorResponse_ShouldRedirect_ForHtmlRequests()
		{
			// Arrange
			var context = CreateTestContext("text/html");
			_mockNext.Invoke(context).Returns(x =>
			{
				context.Response.StatusCode = 404;
				return Task.CompletedTask;
			});

			var middleware = new CustomErrorHandlingMiddleware(_mockNext, _mockEnv, _mockLogger);

			// Act
			await middleware.InvokeAsync(context);

			// Assert
			Assert.Equal(302, context.Response.StatusCode); // Redirect
			Assert.Equal("/Error?statusCode=404", context.Response.Headers["Location"]);
		}
	}
}