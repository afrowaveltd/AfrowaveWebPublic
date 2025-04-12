using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

/// <summary>
/// Tests the PasswordRulesModel class, ensuring proper redirection and validation of password rules during GET and POST
/// requests.
/// Validates installation state and model state for correct behavior.
/// </summary>
public class PasswordRulesModelTests : RazorPageTestBase<PasswordRulesModel>
{
	/// <summary>
	/// Configures services for dependency injection by setting up various service instances with mock implementations.
	/// </summary>
	/// <param name="services">Facilitates the registration of service instances into the application's service collection for later use.</param>
	protected override void ConfigureServices(IServiceCollection services)
	{
		var settingsService = Substitute.For<ISettingsService>();
		settingsService.GetSettingsAsync().Returns(new IdentificatorSettings());

		var statusService = Substitute.For<IInstallationStatusService>();
		statusService.ProperInstallState(InstalationSteps.PasswordRules).Returns(true);

		var selectOptions = Substitute.For<ISelectOptionsServices>();
		selectOptions.GetBinaryOptionsAsync(Arg.Any<bool>()).Returns(Task.FromResult(new List<SelectListItem>
		{
			new() { Value = "true", Text = "Yes" },
			new() { Value = "false", Text = "No" }
		}));

		services.AddSingleton(settingsService);
		services.AddSingleton(statusService);
		services.AddSingleton(selectOptions);
		services.AddSingleton(Substitute.For<IStringLocalizer<PasswordRulesModel>>());
		services.AddSingleton(Substitute.For<ILogger<PasswordRulesModel>>());
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it redirects when the installation state is invalid.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult that points to the LoginRules page.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldRedirect_WhenInstallStateInvalid()
	{
		var status = Services.GetRequiredService<IInstallationStatusService>();
		status.ProperInstallState(InstalationSteps.PasswordRules).Returns(false);

		var page = CreatePageModel();
		var result = await page.OnGetAsync();

		var redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/LoginRules", redirect.PageName);
	}

	/// <summary>
	/// Tests the OnGetAsync method to ensure it returns a page when the installation state is valid. Validates that
	/// certain input properties are not null or empty.
	/// </summary>
	/// <returns>Returns a PageResult indicating the success of the page retrieval.</returns>
	[Fact]
	public async Task OnGetAsync_ShouldReturnPage_WhenInstallStateValid()
	{
		var page = CreatePageModel();
		var result = await page.OnGetAsync();

		Assert.IsType<PageResult>(result);
		Assert.NotNull(page.Input);
		Assert.NotEmpty(page.RequireDigit);
		Assert.NotEmpty(page.RequireLowercase);
		Assert.NotEmpty(page.RequireUppercase);
		Assert.NotEmpty(page.RequireNonAlphanumeric);
	}

	/// <summary>
	/// Handles the post request for a page model and checks the behavior when the model state is invalid. It verifies that
	/// the result is a PageResult.
	/// </summary>
	/// <returns>Returns a PageResult when the model state contains errors.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		var page = CreatePageModel();
		page.ModelState.AddModelError("MinimumLength", "Required");

		var result = await page.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

	/// <summary>
	/// Tests the OnPostAsync method to ensure it redirects correctly when valid input is provided.
	/// </summary>
	/// <returns>Returns a RedirectToPageResult indicating the next page to navigate to.</returns>
	[Fact]
	public async Task OnPostAsync_ShouldRedirect_WhenValid()
	{
		var settingsService = Services.GetRequiredService<ISettingsService>();
		var capturedSettings = new IdentificatorSettings();
		await settingsService.SetSettingsAsync(Arg.Do<IdentificatorSettings>(s =>
		{
			capturedSettings = s;
		}));

		var page = CreatePageModel();
		page.Input = new PasswordRulesModel.InputModel
		{
			MinimumLength = 8,
			MaximumLength = 64,
			RequireDigit = true,
			RequireLowercase = true,
			RequireUppercase = false,
			RequireNonAlphanumeric = false
		};

		page.ModelState.Clear();

		var result = await page.OnPostAsync();

		var redirect = Assert.IsType<RedirectToPageResult>(result);
		Assert.Equal("/Install/CookieSettings", redirect.PageName);
		Assert.True(capturedSettings.PasswordRules.IsConfigured);
		Assert.Equal(8, capturedSettings.PasswordRules.MinimumLength);
	}
}