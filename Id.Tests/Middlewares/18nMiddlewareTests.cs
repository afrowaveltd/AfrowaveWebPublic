﻿namespace Id.Tests.Middlewares
{
	/// <summary>
	/// Unit tests for the <see cref="I18nMiddleware"/> class.
	/// </summary>
	public class I18nMiddlewareTests
	{
		private readonly ICookieService _mockCookieService;
		private readonly ILogger<I18nMiddleware> _mockLogger;
		private readonly I18nMiddleware _middleware;

		/// <summary>
		/// Initializes a new instance of the <see cref="I18nMiddlewareTests"/> class.
		/// </summary>
		public I18nMiddlewareTests()
		{
			_mockCookieService = Substitute.For<ICookieService>();
			_mockLogger = Substitute.For<ILogger<I18nMiddleware>>();
			_middleware = new I18nMiddleware(_mockCookieService, _mockLogger);
		}

		private async Task<HttpContext> CreateTestHttpContext(string acceptLanguage = "en")
		{
			DefaultHttpContext context = new DefaultHttpContext();
			context.Request.Headers["Accept-Language"] = acceptLanguage;
			return await Task.FromResult(context);
		}

		/// <summary>
		/// Verifies that the <see cref="I18nMiddleware"/> sets the culture based on the cookie value.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task InvokeAsync_ShouldSetCultureFromCookie()
		{
			// Arrange
			HttpContext context = await CreateTestHttpContext();
			_ = _mockCookieService.GetCookie("language")
									  .Returns("fr");

			string? actualCulture = null;
			string? actualUICulture = null;

			// Act
			await _middleware.InvokeAsync(context, (ctx) =>
			{
				actualCulture = CultureInfo.CurrentCulture.Name;
				actualUICulture = CultureInfo.CurrentUICulture.Name;
				return Task.CompletedTask;
			});

			// Assert
			Assert.Equal("fr", actualCulture);
			Assert.Equal("fr", actualUICulture);
			_mockCookieService.Received(1).SetCookie("language", "fr");
		}

		/// <summary>
		/// Verifies that the <see cref="I18nMiddleware"/> sets the culture based on the Accept-Language header.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task InvokeAsync_ShouldSetDefaultCulture_WhenNoCookieOrHeader()
		{
			// Arrange
			CultureInfo.CurrentCulture = new CultureInfo("en");
			CultureInfo.CurrentUICulture = new CultureInfo("en");
			HttpContext context = await CreateTestHttpContext("");
			_ = _mockCookieService.GetCookie("language").Returns((string)null);

			// Act
			await _middleware.InvokeAsync(context, (ctx) => Task.CompletedTask);

			// Assert
			Assert.Equal("en", CultureInfo.CurrentCulture.Name);
			Assert.Equal("en", CultureInfo.CurrentUICulture.Name);
		}

		/// <summary>
		/// Verifies that the <see cref="I18nMiddleware"/> sets the culture based on the Accept-Language header.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task InvokeAsync_ShouldUseAcceptLanguageHeader_WhenNoCookie()
		{
			// Arrange
			HttpContext context = await CreateTestHttpContext("es");
			_ = _mockCookieService.GetCookie("language").Returns("");

			string? actualCulture = null;
			string? actualUICulture = null;

			// Act
			await _middleware.InvokeAsync(context, (ctx) =>
			{
				actualCulture = CultureInfo.CurrentCulture.Name;
				actualUICulture = CultureInfo.CurrentUICulture.Name;
				return Task.CompletedTask;
			});

			// Assert
			Assert.Equal("es", actualCulture);
			Assert.Equal("es", actualUICulture);
			_mockCookieService.Received(1).SetCookie("language", "es");
		}

		/// <summary>
		/// Verifies that the <see cref="I18nMiddleware"/> sets the culture to English when the cookie value is invalid.
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task InvokeAsync_ShouldFallbackToEnglish_WhenCultureIsInvalid()
		{
			// Arrange
			CultureInfo.CurrentCulture = new CultureInfo("en");
			CultureInfo.CurrentUICulture = new CultureInfo("en");
			HttpContext context = await CreateTestHttpContext("invalid-culture");
			_ = _mockCookieService.GetCookie("language").Returns("invalid-culture");

			// Act
			await _middleware.InvokeAsync(context, (ctx) => Task.CompletedTask);

			// Assert
			Assert.Equal("en", CultureInfo.CurrentCulture.Name);
			Assert.Equal("en", CultureInfo.CurrentUICulture.Name);
		}
	}
}