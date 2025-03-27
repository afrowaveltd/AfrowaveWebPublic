using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Tests.Pages.Install;

public class PasswordRulesModelTests : RazorPageTestBase<PasswordRulesModel>
{
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

	[Fact]
	public async Task OnPostAsync_ShouldReturnPage_WhenModelInvalid()
	{
		var page = CreatePageModel();
		page.ModelState.AddModelError("MinimumLength", "Required");

		var result = await page.OnPostAsync();

		Assert.IsType<PageResult>(result);
	}

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