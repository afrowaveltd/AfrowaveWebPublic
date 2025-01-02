using Id.Models.SettingsModels;

namespace Id.Services
{
	public interface ISettingsService
	{
		Task<IdentificatorSettings> GetSettingsAsync();

		Task SetApplicationId(string id);

		Task SetSettingsAsync(IdentificatorSettings settings);
	}
}