using SharedTools.Models.DatabaseModels;

namespace Id.Tests.SharedTools.Tests.Models.DatabaseModels;

/// <summary>
/// Tests for the ExcludedLanguage class.
/// </summary>
public class ExcludedLanguageTests
{
	/// <summary>
	/// Tests the creation of an ExcludedLanguage object with valid data.
	/// </summary>
	[Fact]
	public void CanCreate_ExcludedLanguage_WithValidData()
	{
		// Arrange
		ExcludedLanguage excluded = new ExcludedLanguage
		{
			ObjectId = "app-123",
			ObjectType = "ApplicationDescription",
			LanguageCode = "cs"
		};

		// Assert
		Assert.Equal("app-123", excluded.ObjectId);
		Assert.Equal("ApplicationDescription", excluded.ObjectType);
		Assert.Equal("cs", excluded.LanguageCode);
	}

	/// <summary>
	/// Tests the creation of an ExcludedLanguage object with invalid data.
	/// </summary>
	/// <param name="type">string representing text value of the enum</param>
	/// <param name="id">the object Id</param>
	/// <param name="lang">Two letters language code</param>
	[Theory]
	[InlineData("BrandDescription", "789", "fr")]
	[InlineData("UserDescription", "user-42", "ar")]
	public void CanAssign_MultipleExcludedLanguage_Values(string type, string id, string lang)
	{
		// Arrange
		ExcludedLanguage excluded = new ExcludedLanguage
		{
			ObjectType = type,
			ObjectId = id,
			LanguageCode = lang
		};

		// Assert
		Assert.Equal(type, excluded.ObjectType);
		Assert.Equal(id, excluded.ObjectId);
		Assert.Equal(lang, excluded.LanguageCode);
	}

	[Fact]
	public void DefaultConstructor_ShouldInitializeFields()
	{
		ExcludedLanguage excluded = new ExcludedLanguage();

		Assert.NotNull(excluded.ObjectId);
		Assert.NotNull(excluded.ObjectType);
		Assert.NotNull(excluded.LanguageCode);
	}
}