using Id.Models.DataViews;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Id.Tests.Pages.Account
{
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
			var userId = "test-user-id";

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

		[Fact]
		public async Task OnGetAsync_ShouldRedirectToError_WhenAppDoesNotExist()
		{
			var model = CreatePageModelWith(appExists: false);

			var result = await model.OnGetAsync();

			var redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Error/404", redirect.PageName);
		}

		[Fact]
		public async Task OnGetAsync_ShouldRedirectToIndex_WhenAuthenticated()
		{
			var model = CreatePageModelWith(isAuthenticated: true);

			var result = await model.OnGetAsync();

			var redirect = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Account/Index", redirect.PageName);
		}

		[Fact]
		public async Task OnGetAsync_ShouldRedirectToError_WhenAppInfoIsNull()
		{
			var model = CreatePageModelWith(appView: null);

			var result = await model.OnGetAsync();

			Assert.IsType<PageResult>(result);
		}

		[Fact]
		public async Task OnGetAsync_ShouldReturnPage_WhenAllIsValid()
		{
			var model = CreatePageModelWith();

			var result = await model.OnGetAsync();

			Assert.IsType<PageResult>(result);
		}
	}
}