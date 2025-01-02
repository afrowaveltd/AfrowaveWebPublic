public interface IEncryptionService
{
	string GenerateApplicationSecret();

	byte[] GetJwtSigningKey(string key);

	Task<string> HashPasswordAsync(string password);

	bool VerifyPassword(string password, string hashedPassword);
}