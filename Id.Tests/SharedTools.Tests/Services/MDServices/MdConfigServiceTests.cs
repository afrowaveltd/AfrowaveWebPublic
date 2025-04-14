using SharedTools.Models.MdModels;
using System.Text.Json;

namespace Id.Tests.SharedTools.Tests.Services.MdServices;

/// <summary>
/// Unit tests for the Markdown configuration service.
/// </summary>
public class MdConfigServiceTests
{
	private const string TestDir = "TestData/MdMappings/";
	private readonly string _masterPath = Path.Combine(TestDir, "MarkdownMappings.master.json");
	private readonly string _userPath = Path.Combine(TestDir, "MarkdownMappings.user.json");

	/// <summary>
	/// Initializes the test class and sets up the test environment by creating necessary directories and files.
	/// </summary>
	public MdConfigServiceTests()
	{
		_ = Directory.CreateDirectory(TestDir);

		File.WriteAllText(_masterPath, """
        [
            { "MdStart": "#", "HtmlElement": "h1" },
            { "MdStart": "**", "HtmlElement": "strong" }
        ]
        """);

		if(File.Exists(_userPath))
		{
			File.Delete(_userPath);
		}
	}

	/// <summary>
	/// Tests the GetCombinedMappingsAsync method to ensure it correctly merges master and user mappings.
	/// </summary>
	/// <returns></returns>
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

	/// <summary>
	/// Tests the GetCombinedMappingsAsync method when the user file does not exist.
	/// </summary>
	/// <returns></returns>
	[Fact]
	public async Task SaveOrUpdateMappingAsync_ShouldAddAndUpdate()
	{
		IMdConfigService service = new MdConfigService(_masterPath, _userPath);

		MdElementMapping newItem = new MdElementMapping
		{
			MdStart = "*",
			HtmlElement = "em",
			CssClass = "italic"
		};

		await service.SaveOrUpdateMappingAsync(newItem);

		List<MdElementMapping> loaded = await LoadFromUserFile();

		_ = Assert.Single(loaded);
		Assert.Equal("*", loaded[0].MdStart);

		// Update
		newItem.CssClass = "emphasis";
		await service.SaveOrUpdateMappingAsync(newItem);

		loaded = await LoadFromUserFile();
		_ = Assert.Single(loaded);
		Assert.Equal("emphasis", loaded[0].CssClass);
	}

	/// <summary>
	/// Tests the DeleteUserMappingAsync method to ensure it correctly removes a mapping from the user file.
	/// </summary>
	/// <returns></returns>
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
		_ = Assert.Single(loaded);
		Assert.Equal("-", loaded[0].MdStart);
	}

	/// <summary>
	/// Tests the ResetUserOverridesAsync method to ensure it deletes the user file.
	/// </summary>
	/// <returns></returns>
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