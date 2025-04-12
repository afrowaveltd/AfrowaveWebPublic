using SharedTools.Models.MdModels;

public interface IMdConfigService
{
	Task<List<MdElementMapping>> GetCombinedMappingsAsync();

	Task ResetUserOverridesAsync();

	Task SaveOrUpdateMappingAsync(MdElementMapping mapping);

	Task DeleteUserMappingAsync(string mdStart);
}