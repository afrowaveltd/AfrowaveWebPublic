using SharedTools.Models.MdModels;

/// <summary>
/// Provides methods for managing mappings in a configuration service. Includes functionality to retrieve, save, update,
/// and delete mappings.
/// </summary>
public interface IMdConfigService
{
	/// <summary>
	/// Retrieves a combined list of MdElementMapping objects asynchronously.
	/// </summary>
	/// <returns>Returns a task that represents the asynchronous operation, containing a list of MdElementMapping.</returns>
	Task<List<MdElementMapping>> GetCombinedMappingsAsync();

	/// <summary>
	/// Resets any user-specific overrides asynchronously.
	/// </summary>

	void ResetUserOverridesAsync();

	/// <summary>
	/// Saves or updates settings
	/// </summary>
	/// <param name="mapping"></param>
	/// <returns>Returns a task that represents the asynchronous operation, containing a list of MDElementMapping.</returns>
	Task SaveOrUpdateMappingAsync(MdElementMapping mapping);

	/// <summary>
	/// Deletes the user mapping
	/// </summary>
	/// <param name="mdStart"></param>
	/// <returns>Returns a task that represents the asynchronous operation</returns>
	Task DeleteUserMappingAsync(string mdStart);
}