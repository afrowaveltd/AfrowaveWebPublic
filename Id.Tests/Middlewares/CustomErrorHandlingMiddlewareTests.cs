using Microsoft.AspNetCore.Http;

namespace Id.Tests.Middlewares
{
	/// <summary>
	/// Unit tests for the <see cref="CustomErrorHandlingMiddleware"/> class.
	/// </summary>
	public class CustomErrorHandlingMiddlewareTests
	{
		private readonly Mock<RequestDelegate> _mockNext;
		private readonly Mock<ILogger<CustomErrorHandlingMiddleware>> _mockLogger;
		private readonly Mock<IWebHostEnvironment> _mockEnv;
		private readonly Mock<IStringLocalizer<CustomErrorHandlingMiddleware>> _mockLocalizer;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomErrorHandlingMiddlewareTests"/> class.
		/// </summary>
		public CustomErrorHandlingMiddlewareTests()
		{
			_mockNext = new Mock<RequestDelegate>();
			_mockLogger = new Mock<ILogger<CustomErrorHandlingMiddleware>>();
			_mockEnv = new Mock<IWebHostEnvironment>();
			_mockLocalizer = new Mock<IStringLocalizer<CustomErrorHandlingMiddleware>>();

			_ = _mockEnv.Setup(e => e.EnvironmentName).Returns("Production");
			_ = _mockLocalizer.Setup(l => l[It.IsAny<string>()])
				 .Returns<string>(key => new LocalizedString(key, key switch
				 {
					 "Error_404_Title" => "Not Found",
					 "Error_500_Title" => "Server Error",
					 "Error_Generic_Title" => "Error",
					 _ => key
				 }));
		}

		private HttpContext CreateTestContext(string acceptHeader = "application/json")
		{
			DefaultHttpContext context = new DefaultHttpContext();
			context.Response.Body = new MemoryStream();
			context.Request.Headers["Accept"] = acceptHeader;

			ServiceCollection services = new ServiceCollection();
			_ = services.AddSingleton(_mockLocalizer.Object);
			context.RequestServices = services.BuildServiceProvider();

			return context;
		}

		/// <summary>
		/// Verifies that the <see cref="CustomErrorHandlingMiddleware"/> constructor initializes the instance correctly.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task HandleErrorResponse_ShouldRedirect_ForHtmlRequests()
		{
			// Arrange
			HttpContext context = CreateTestContext("text/html");
			_ = _mockNext.Setup(n => n(context)).Callback(() => context.Response.StatusCode = 404);
			CustomErrorHandlingMiddleware middleware = new CustomErrorHandlingMiddleware(_mockNext.Object, _mockEnv.Object, _mockLogger.Object);

			// Act
			await middleware.InvokeAsync(context);

			// Assert
			Assert.Equal(302, context.Response.StatusCode); // Redirect status code
			Assert.Equal("/Error?statusCode=404", context.Response.Headers["Location"]);
		}
	}
}