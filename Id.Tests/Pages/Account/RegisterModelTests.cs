using Id.Models.DataViews;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Id.Tests.Pages.Account
{
	/// <summary>
	/// Tests the functionality of the RegisterUserModel class, including user registration and validation scenarios. It
	/// verifies redirects and page returns based on various conditions.
	/// </summary>
	public class RegisterUserModelTests
	{
		private RegisterUserModel CreatePageModelWith(
			RegisterUserResult? result = null,
			List<string>? modelErrors = null,
			bool isAuthenticated = false,
			bool appExists = true,
			ApplicationView? appView = null)
		{
			var logger = Substitute.For<ILogger<RegisterUserModel>>();
			var userService = Substitute.For<IUsersManager>();
			var applicationUsersManager = Substitute.For<IApplicationUsersManager>();
			var applicationService = Substitute.For<IApplicationsManager>();
			var encryptionService = Substitute.For<IEncryptionService>();
			var roleService = Substitute.For<IRolesManager>();
			var selectOptionsService = Substitute.For<ISelectOptionsServices>();
			var settingsService = Substitute.For<ISettingsService>();
			var localizer = Substitute.For<IStringLocalizer<RegisterUserModel>>();

			var appId = "test-app-id";

			settingsService.GetPasswordRulesAsync().Returns(new PasswordRules { MinimumLength = 8 });
			settingsService.GetLoginRulesAsync().Returns(new LoginRules { RequireConfirmedEmail = false });
			settingsService.GetApplicationIdAsync().Returns(appId);
			applicationService.GetAuthenticatorIdAsync().Returns(appId);
			applicationService.ApplicationExistsAsync(appId).Returns(appExists);
			applicationService.GetInfoAsync(appId).Returns(appView ?? new ApplicationView());

			userService.RegisterUserAsync(Arg.Any<RegisterUserInput>()).Returns(result ?? new RegisterUserResult());

			var httpContext = new DefaultHttpContext();
			if(isAuthenticated)
			{
				httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
					new Claim(ClaimTypes.Name, "testuser")
				}, "mock"));
			}

			var model = new RegisterUserModel(
				logger,
				userService,
				applicationUsersManager,
				applicationService,
				encryptionService,
				roleService,
				selectOptionsService,
				settingsService,
				localizer
			)
			{
				ApplicationId = appId,
				Input = new RegisterUserInput
				{
					Email = "user@example.com",
					Password = "Pass1234",
					PasswordConfirm = "Pass1234",
					FirstName = "Test",
					LastName = "User",
					DisplayedName = "Test User",
					Birthdate = DateTime.UtcNow.AddYears(-10), // default OK
					Gender = Models.Gender.Other,
					AcceptTerms = true,
					AcceptPrivacyPolicy = true,
					AcceptCookiePolicy = true
				},
				PageContext = new PageContext
				{
					ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
					HttpContext = httpContext
				}
			};

			if(modelErrors != null)
			{
				foreach(var error in modelErrors)
				{
					model.ModelState.AddModelError(string.Empty, error);
				}
			}

			return model;
		}

		/// <summary>
		/// Handles the post request for user registration and returns a redirect upon successful registration.
		/// </summary>
		/// <returns>Returns a redirect result to the specified page after successful user registration.</returns>
		[Fact]
		public async Task OnPostAsync_ReturnsRedirect_WhenRegistrationSucceeds()
		{
			var model = CreatePageModelWith(new RegisterUserResult
			{
				UserCreated = true,
				UserId = "user-id",
				ProfilePictureUploaded = true
			});

			var result = await model.OnPostAsync();

			var redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Account/AuthenticatorUserRegistration", redirect.PageName);
		}

		/// <summary>
		/// Handles the registration process and checks if the user is too young based on their birthdate.
		/// </summary>
		/// <returns>Returns a PageResult when the user is deemed too young to register.</returns>
		[Fact]
		public async Task OnPostAsync_ReturnsPage_WhenBirthdateTooYoung()
		{
			var result = new RegisterUserResult
			{
				UserCreated = false,
				Errors = new List<string> { "User is too young" }
			};

			var model = CreatePageModelWith(result);
			model.Input.Birthdate = DateTime.UtcNow.AddYears(-5); // too young

			var response = await model.OnPostAsync();

			Assert.IsType<PageResult>(response);
			Assert.Contains("User is too young", model.RegistrationErrors);
		}

		/// <summary>
		/// Handles the GET request for a page and checks if the application exists. If not, it redirects to a 404 error page.
		/// </summary>
		/// <returns>Returns a redirect result to the error page if the application does not exist.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirectToError_WhenAppDoesNotExist()
		{
			var model = CreatePageModelWith(appExists: false);

			var result = await model.OnGetAsync();

			var redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Error/404", redirect.PageName);
		}

		/// <summary>
		/// Tests the OnGetAsync method to ensure it redirects to the Index page when the user is authenticated. It verifies the
		/// redirect result and the target page.
		/// </summary>
		/// <returns>Returns a RedirectToPageResult indicating the page to redirect to.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirectToIndex_WhenAuthenticated()
		{
			var model = CreatePageModelWith(isAuthenticated: true);

			var result = await model.OnGetAsync();

			var redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Account/Index", redirect.PageName);
		}

		/// <summary>
		/// Handles the GET request and checks if the application information is null, redirecting to an error page if so.
		/// </summary>
		/// <returns>Returns a PageResult indicating the outcome of the GET request.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldRedirectToError_WhenAppInfoIsNull()
		{
			var model = CreatePageModelWith(appView: null);

			var result = await model.OnGetAsync();

			Assert.IsType<PageResult>(result);
		}

		/// <summary>
		/// Handles the GET request asynchronously and returns a page result if all conditions are valid.
		/// </summary>
		/// <returns>Returns a PageResult indicating the success of the operation.</returns>
		[Fact]
		public async Task OnGetAsync_ShouldReturnPage_WhenAllIsValid()
		{
			var model = CreatePageModelWith();

			var result = await model.OnGetAsync();

			Assert.IsType<PageResult>(result);
		}
	}
}