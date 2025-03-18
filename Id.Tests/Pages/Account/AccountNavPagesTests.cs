using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Id.Tests.Pages.Account;

/// <summary>
/// Unit tests for the <see cref="AccountNavPages"/> class.
/// </summary>
public class AccountNavPagesTests
{
	/// <summary>
	/// Verifies that the navigation properties return the correct paths.
	/// </summary>
	/// <param name="propertyName"></param>
	/// <param name="expectedPath"></param>

	[Theory]
	[InlineData("Index", "./Index")]
	[InlineData("Register", "./Register")]
	[InlineData("Login", "./Login")]
	[InlineData("Logout", "./Login")]
	[InlineData("Profile", "./Profile")]
	[InlineData("PrivacySettings", "./Privacy")]
	[InlineData("ChangeEmail", "./Email")]
	[InlineData("ChangePassword", "./Password")]
	[InlineData("PersonalData", "./PersonalData")]
	[InlineData("DeleteAccount", "./DeleteAccount")]
	public void NavigationProperties_ShouldReturnCorrectPaths(string propertyName, string expectedPath)
	{
		// Act
		string? actualValue = typeof(AccountNavPages).GetProperty(propertyName)?.GetValue(null) as string;

		// Assert
		Assert.Equal(expectedPath, actualValue);
	}

	/// <summary>
	/// Verifies that the page navigation class is set correctly.
	/// </summary>
	[Fact]
	public void PageNavClass_ShouldReturnActive_WhenPageMatches()
	{
		// Arrange
		ViewContext viewContext = new ViewContext
		{
			ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
			{
				["ActivePage"] = "./Profile"
			}
		};

		// Act
		string result = AccountNavPages.ProfileNavClass(viewContext);

		// Assert
		Assert.Equal("active", result);
	}

	/// <summary>
	/// Verifies that the page navigation class is set correctly.
	/// </summary>
	[Fact]
	public void PageNavClass_ShouldReturnEmpty_WhenPageDoesNotMatch()
	{
		// Arrange
		ViewContext viewContext = new ViewContext
		{
			ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
			{
				["ActivePage"] = "./Register"
			}
		};

		// Act
		string result = AccountNavPages.ProfileNavClass(viewContext);

		// Assert
		Assert.Equal("", result);
	}
}