namespace Id.Tests.Middlewares
{
	/// <summary>
	/// Unit tests for the <see cref="CustomErrorHandlingMiddleware"/> class.
	/// </summary>
	public class CustomErrorHandlingMiddlewareTests
	{
		private readonly RequestDelegate _mockNext;
		private readonly ILogger<CustomErrorHandlingMiddleware> _mockLogger;
		private readonly IWebHostEnvironment _mockEnv;
		private readonly IStringLocalizer<CustomErrorHandlingMiddleware> _mockLocalizer;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomErrorHandlingMiddlewareTests"/> class.
		/// </summary>
		public CustomErrorHandlingMiddlewareTests()
		{
			_mockNext = Substitute.For<RequestDelegate>();
			_mockLogger = Substitute.For<ILogger<CustomErrorHandlingMiddleware>>();
			_mockEnv = Substitute.For<IWebHostEnvironment>();
			_mockLocalizer = Substitute.For<IStringLocalizer<CustomErrorHandlingMiddleware>>();

			_ = _mockEnv.EnvironmentName.Returns("Production");
			_ = _mockLocalizer[Arg.Any<string>()].Returns(callInfo =>
				new LocalizedString(callInfo.Arg<string>(), callInfo.Arg<string>() switch
				{
					"Error_404_Title" => "Not Found",
					"Error_500_Title" => "Server Error",
					"Error_Generic_Title" => "Error",
					_ => callInfo.Arg<string>()
				}));
		}

		private HttpContext CreateTestContext(string acceptHeader = "application/json")
		{
			DefaultHttpContext context = new DefaultHttpContext();
			context.Response.Body = new MemoryStream();
			context.Request.Headers["Accept"] = acceptHeader;

			ServiceCollection services = new ServiceCollection();
			_ = services.AddSingleton(_mockLocalizer);
			context.RequestServices = services.BuildServiceProvider();

			return context;
		}

		/// <summary>
		/// Verifies that the <see cref="CustomErrorHandlingMiddleware"/> constructor initializes the instance correctly.
		/// </summary>
		[Fact]
		public async Task HandleErrorResponse_ShouldRedirect_ForHtmlRequests()
		{
			// Arrange
			HttpContext context = CreateTestContext("text/html");
			_ = _mockNext.Invoke(context).Returns(Task.Run(() => context.Response.StatusCode = 404));
			CustomErrorHandlingMiddleware middleware = new CustomErrorHandlingMiddleware(_mockNext, _mockEnv, _mockLogger);

			// Act
			await middleware.InvokeAsync(context);

			// Assert
			Assert.Equal(302, context.Response.StatusCode); // Redirect status code
			Assert.Equal("/Error?statusCode=404", context.Response.Headers["Location"]);
		}
	}
}