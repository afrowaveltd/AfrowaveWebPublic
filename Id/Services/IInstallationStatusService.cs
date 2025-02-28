namespace Id.Services
{
	/// <summary>
	/// Service to handle installation status.
	/// </summary>
	public interface IInstallationStatusService
	{
		/// <summary>
		/// Get the current installation step.
		/// </summary>
		/// <returns>InstalationSteps</returns>
		Task<InstalationSteps> GetInstallationStepAsync();

		/// <summary>
		/// Checks if the installation is in the correct state.
		/// </summary>
		/// <param name="actualStep">InstalationStep which is actually being executed</param>
		/// <returns>True if the ActualStep is equal to expected step</returns>
		Task<bool> ProperInstallState(InstalationSteps actualStep);
	}
}