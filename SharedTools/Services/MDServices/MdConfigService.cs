using SharedTools.Models.MdModels;
using System.Text.Json;

namespace SharedTools.Services.MdServices;

public class MdConfigService : IMdConfigService
{
	private readonly string _masterPath;
	private readonly string _userPath;

	public MdConfigService(
		 string masterPath = "SharedTools/Settings/MarkdownMappings.master.json",
		 string userPath = "SharedTools/Settings/MarkdownMappings.user.json")
	{
		_masterPath = masterPath;
		_userPath = userPath;
	}

	public async Task<List<MdElementMapping>> GetCombinedMappingsAsync()
	{
		List<MdElementMapping> master = await LoadJsonAsync(_masterPath);
		List<MdElementMapping> user = File.Exists(_userPath)
			 ? await LoadJsonAsync(_userPath)
			 : [];

		var combined = master.ToDictionary(e => e.MdStart);
		foreach(var userItem in user)
		{
			combined[userItem.MdStart] = userItem; // override
		}

		return combined.Values.ToList();
	}

	public async Task ResetUserOverridesAsync()
	{
		if(File.Exists(_userPath))
		{
			File.Delete(_userPath);
		}
	}

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

	public async Task DeleteUserMappingAsync(string mdStart)
	{
		if(!File.Exists(_userPath)) return;

		List<MdElementMapping> userList = await LoadJsonAsync(_userPath);
		userList.RemoveAll(e => e.MdStart == mdStart);

		if(userList.Count == 0)
			File.Delete(_userPath);
		else
			await SaveJsonAsync(_userPath, userList);
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