using SharedTools.Models.MdModels;
using System.Text.Json;

namespace SharedTools.Services.MdServices;

/// <summary>
/// Manages Markdown element mappings by loading, saving, and updating user and master configurations. Supports
/// combining, resetting, and deleting mappings.
/// </summary>
public class MdConfigService : IMdConfigService
{
	private readonly string _masterPath;
	private readonly string _userPath;

	/// <summary>
	/// Initializes the configuration service for Markdown mappings with specified file paths for master and user settings.
	/// </summary>
	/// <param name="masterPath">Specifies the file path for the master settings configuration.</param>
	/// <param name="userPath">Specifies the file path for the user settings configuration.</param>
	public MdConfigService(
		 string masterPath = "SharedTools/Settings/MarkdownMappings.master.json",
		 string userPath = "SharedTools/Settings/MarkdownMappings.user.json")
	{
		_masterPath = masterPath;
		_userPath = userPath;
	}

	/// <summary>
	/// Combines mappings from a master JSON file and an optional user JSON file. User mappings override master mappings
	/// when they share the same key.
	/// </summary>
	/// <returns>A list of MdElementMapping objects containing the combined mappings.</returns>
	public async Task<List<MdElementMapping>> GetCombinedMappingsAsync()
	{
		List<MdElementMapping> master = await LoadJsonAsync(_masterPath);
		List<MdElementMapping> user = File.Exists(_userPath)
			 ? await LoadJsonAsync(_userPath)
			 : [];

		Dictionary<string, MdElementMapping> combined = master.ToDictionary(e => e.MdStart);
		foreach(MdElementMapping userItem in user)
		{
			combined[userItem.MdStart] = userItem; // override
		}

		return combined.Values.ToList();
	}

	/// <summary>
	/// Deletes the user overrides file if it exists, effectively resetting any user-specific settings.
	/// </summary>
	/// <returns>This method does not return a value.</returns>
	public void ResetUserOverridesAsync()
	{
		if(File.Exists(_userPath))
		{
			File.Delete(_userPath);
		}
	}

	/// <summary>
	/// Saves or updates the mappings file
	/// </summary>
	/// <param name="mapping"></param>
	/// <returns></returns>
	public async Task SaveOrUpdateMappingAsync(MdElementMapping mapping)
	{
		List<MdElementMapping> userList = File.Exists(_userPath)
			 ? await LoadJsonAsync(_userPath)
			 : [];

		int index = userList.FindIndex(e => e.MdStart == mapping.MdStart);
		if(index >= 0)
		{
			userList[index] = mapping;
		}
		else
		{
			userList.Add(mapping);
		}

		await SaveJsonAsync(_userPath, userList);
	}

	/// <summary>
	/// Deletes the user mapping fromt he file
	/// </summary>
	/// <param name="mdStart"></param>
	/// <returns></returns>
	public async Task DeleteUserMappingAsync(string mdStart)
	{
		if(!File.Exists(_userPath))
		{
			return;
		}

		List<MdElementMapping> userList = await LoadJsonAsync(_userPath);
		_ = userList.RemoveAll(e => e.MdStart == mdStart);

		if(userList.Count == 0)
		{
			File.Delete(_userPath);
		}
		else
		{
			await SaveJsonAsync(_userPath, userList);
		}
	}

	private async Task<List<MdElementMapping>> LoadJsonAsync(string path)
	{
		using FileStream stream = File.OpenRead(path);
		return await JsonSerializer.DeserializeAsync<List<MdElementMapping>>(stream)
				 ?? [];
	}

	private async Task SaveJsonAsync(string path, List<MdElementMapping> list)
	{
		using FileStream stream = File.Create(path);
		await JsonSerializer.SerializeAsync(stream, list, new JsonSerializerOptions { WriteIndented = true });
	}
}