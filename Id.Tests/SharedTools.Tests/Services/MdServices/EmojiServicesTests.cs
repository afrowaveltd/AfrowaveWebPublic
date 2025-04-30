namespace Id.Tests.SharedTools.Tests.Services;

public class EmojiServiceTests
{
	[Fact]
	public void GetByUtf_Returns_Correct_String()
	{
		EmojiService service = new EmojiService();
		string emoji = service.GetByUnicode("1f600");

		Assert.NotNull(emoji);
		Assert.Equal("\uD83D\uDE00", emoji);
	}

	[Fact]
	public void GetByName_Returns_Correct_String()
	{
		EmojiService service = new EmojiService();
		string emoji = service.GetByName("grinning face");

		Assert.NotNull(emoji);
		Assert.Equal("\uD83D\uDE00", emoji);
	}

	[Fact]
	public void Get_Returns_Same_From_Name_And_Utf()
	{
		EmojiService service = new EmojiService();

		string byName = service.Get("grinning face");
		string byUtf = service.Get("1f600");

		Assert.NotNull(byName);
		Assert.NotNull(byUtf);
		Assert.Equal(byName, byUtf);
	}

	[Fact]
	public void Get_Invalid_Returns_Empty()
	{
		EmojiService service = new EmojiService();
		string emoji = service.Get("non-existent-emoji");

		Assert.Equal(string.Empty, emoji);
	}
}