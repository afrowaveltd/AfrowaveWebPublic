using SharedTools.Models.MdModels;
using SharedTools.Services.MdServices;
using System.Text.Json;

namespace Id.Tests.SharedTools.Tests.Services.MdServices;

public class MdConfigServiceTests
{
	private const string TestDir = "TestData/MdMappings/";
	private readonly string _masterPath = Path.Combine(TestDir, "MarkdownMappings.master.json");
	private readonly string _userPath = Path.Combine(TestDir, "MarkdownMappings.user.json");

	public MdConfigServiceTests()
	{
		Directory.CreateDirectory(TestDir);

		File.WriteAllText(_masterPath, """
        [
            { "MdStart": "#", "HtmlElement": "h1" },
            { "MdStart": "**", "HtmlElement": "strong" }
        ]
        """);

		if(File.Exists(_userPath))
			File.Delete(_userPath);
	}

	[Fact]
	public async Task GetCombinedMappingsAsync_ShouldMergeCorrectly()
	{
		File.WriteAllText(_userPath, """
        [
            { "MdStart": "**", "HtmlElement": "b", "CssClass": "custom-bold" }
        ]
        """);

		IMdConfigService service = new MdConfigService(_masterPath, _userPath);

		List<MdElementMapping> result = await service.GetCombinedMappingsAsync();

		Assert.Equal(2, result.Count);
		Assert.Contains(result, x => x.MdStart == "#" && x.HtmlElement == "h1");
		Assert.Contains(result, x => x.MdStart == "**" && x.HtmlElement == "b" && x.CssClass == "custom-bold");
	}

	[Fact]
	public async Task SaveOrUpdateMappingAsync_ShouldAddAndUpdate()
	{
		IMdConfigService service = new MdConfigService(_masterPath, _userPath);

		var newItem = new MdElementMapping
		{
			MdStart = "*",
			HtmlElement = "em",
			CssClass = "italic"
		};

		await service.SaveOrUpdateMappingAsync(newItem);

		List<MdElementMapping> loaded = await LoadFromUserFile();

		Assert.Single(loaded);
		Assert.Equal("*", loaded[0].MdStart);

		// Update
		newItem.CssClass = "emphasis";
		await service.SaveOrUpdateMappingAsync(newItem);

		loaded = await LoadFromUserFile();
		Assert.Single(loaded);
		Assert.Equal("emphasis", loaded[0].CssClass);
	}

	[Fact]
	public async Task DeleteUserMappingAsync_ShouldRemoveCorrectEntry()
	{
		File.WriteAllText(_userPath, """
        [
            { "MdStart": "-", "HtmlElement": "li" },
            { "MdStart": ">", "HtmlElement": "blockquote" }
        ]
        """);

		IMdConfigService service = new MdConfigService(_masterPath, _userPath);

		await service.DeleteUserMappingAsync(">");

		List<MdElementMapping> loaded = await LoadFromUserFile();
		Assert.Single(loaded);
		Assert.Equal("-", loaded[0].MdStart);
	}

	[Fact]
	public async Task ResetUserOverridesAsync_ShouldDeleteUserFile()
	{
		File.WriteAllText(_userPath, "[]");
		Assert.True(File.Exists(_userPath));

		IMdConfigService service = new MdConfigService(_masterPath, _userPath);
		await service.ResetUserOverridesAsync();

		Assert.False(File.Exists(_userPath));
	}

	private async Task<List<MdElementMapping>> LoadFromUserFile()
	{
		using FileStream fs = File.OpenRead(_userPath);
		return await JsonSerializer.DeserializeAsync<List<MdElementMapping>>(fs) ?? [];
	}
}