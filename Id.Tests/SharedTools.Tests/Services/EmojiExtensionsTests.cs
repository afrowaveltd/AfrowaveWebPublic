using SharedTools.Extensions;

namespace Id.Tests.SharedTools.Tests.Services;

/// <summary>
/// Tests for the EmojiExtensions class.
/// </summary>
public class EmojiExtensionsTests
{
	/// <summary>
	/// Tests the ToEmoji method to ensure it returns the correct emoji string.
	/// </summary>
	[Fact]
	public void ToEmoji_Returns_Valid_String()
	{
		IEmojiService service = Substitute.For<IEmojiService>();
		_ = service.Get("1f600").Returns("\uD83D\uDE00");

		string result = "1f600".ToEmoji(service);

		Assert.Equal("\uD83D\uDE00", result);
	}

	/// <summary>
	/// Tests the ToEmoji method to ensure it returns the correct emoji string for a name.
	/// </summary>
	[Fact]
	public void ToEmoji_Returns_Empty_When_NotFound()
	{
		IEmojiService service = Substitute.For<IEmojiService>();
		_ = service.Get("nonexistent").Returns(string.Empty);

		string result = "nonexistent".ToEmoji(service);

		Assert.Equal(string.Empty, result);
	}
}