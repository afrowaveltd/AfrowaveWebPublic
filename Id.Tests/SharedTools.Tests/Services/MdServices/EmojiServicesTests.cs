namespace Id.Tests.SharedTools.Tests.Services;

/// <summary>
/// EmojiServiceTests class contains unit tests for the EmojiService class.
/// </summary>
public class EmojiServiceTests
{
	/// <summary>
	/// Verifies that the <see cref="EmojiService.GetByUnicode(string)"/> method correctly retrieves the emoji string
	/// corresponding to the specified Unicode value.
	/// </summary>
	/// <remarks>This test ensures that the <see cref="EmojiService"/> returns the expected emoji string when
	/// provided with a valid Unicode code point.</remarks>
	[Fact]
	public void GetByUtf_Returns_Correct_String()
	{
		EmojiService service = new EmojiService();
		string emoji = service.GetByUnicode("1f600");

		Assert.NotNull(emoji);
		Assert.Equal("\uD83D\uDE00", emoji);
	}

	/// <summary>
	/// Verifies that the <see cref="EmojiService.GetByName"/> method returns the correct emoji string for a given emoji
	/// name.
	/// </summary>
	/// <remarks>This test ensures that the <see cref="EmojiService.GetByName"/> method correctly maps the  provided
	/// emoji name to its corresponding Unicode string representation.</remarks>
	[Fact]
	public void GetByName_Returns_Correct_String()
	{
		EmojiService service = new EmojiService();
		string emoji = service.GetByName("grinning_face");

		Assert.NotNull(emoji);
		Assert.Equal("\uD83D\uDE00", emoji);
	}

	/// <summary>
	/// Verifies that the <see cref="EmojiService.Get(string)"/> method returns the same emoji when queried by its name and
	/// by its UTF-16 code point.
	/// </summary>
	/// <remarks>This test ensures that the <see cref="EmojiService"/> correctly maps both emoji names (e.g.,
	/// "grinning_face") and their corresponding UTF-16 code points (e.g., "1f600") to the same emoji string. It validates
	/// that the returned values are non-null, non-empty, and consistent across both query methods.</remarks>
	[Fact]
	public void Get_Returns_Same_From_Name_And_Utf()
	{
		EmojiService service = new EmojiService();

		string byName = service.Get("grinning_face");
		string byUtf = service.Get("1f600");

		Assert.NotNull(byName);
		Assert.NotNull(byUtf);
		Assert.NotEmpty(byName);
		Assert.NotEmpty(byUtf);
		Assert.Equal(byName, byUtf);
	}

	/// <summary>
	/// Verifies that the <see cref="EmojiService.Get(string)"/> method returns an empty string when provided with an
	/// missing or invalid emoji name or Unicode code point.
	/// </summary>
	[Fact]
	public void Get_Invalid_Returns_Empty()
	{
		EmojiService service = new EmojiService();
		string emoji = service.Get("non-existent-emoji");

		Assert.Equal(string.Empty, emoji);
	}
}