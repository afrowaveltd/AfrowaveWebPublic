namespace Id.Services
{
    public interface IInstallationStatusService
    {
        Task<InstalationSteps> GetInstallationStepAsync();

        Task<bool> ProperInstallState(InstalationSteps actualStep);
    }
}