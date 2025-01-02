using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

public class EncryptionService : IEncryptionService
{
	private readonly ILogger<EncryptionService> _logger;
	private readonly string _settingsFilePath;

	public EncryptionService(ILogger<EncryptionService> logger)
	{
		_logger = logger;
		string projectPath = AppDomain.CurrentDomain.BaseDirectory
									  .Substring(0, AppDomain.CurrentDomain.BaseDirectory
									  .IndexOf("bin"));
		_settingsFilePath = Path.Combine(projectPath, "Settings", "settings.bin");
	}

	public async Task<string> HashPasswordAsync(string password)
	{
		byte[] salt = new byte[16];
		RandomNumberGenerator.Fill(salt);

		using Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
		byte[] hashBytes = rfc2898.GetBytes(32);

		byte[] hashWithSalt = new byte[48];
		Array.Copy(salt, 0, hashWithSalt, 0, 16);
		Array.Copy(hashBytes, 0, hashWithSalt, 16, 32);

		return Convert.ToBase64String(hashWithSalt);
	}

	public bool VerifyPassword(string password, string hashedPassword)
	{
		byte[] hashWithSalt = Convert.FromBase64String(hashedPassword);
		byte[] salt = new byte[16];
		byte[] hashBytes = new byte[32];

		Array.Copy(hashWithSalt, 0, salt, 0, 16);
		Array.Copy(hashWithSalt, 16, hashBytes, 0, 32);

		using Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
		byte[] newHashBytes = rfc2898.GetBytes(32);

		return newHashBytes.SequenceEqual(hashBytes);
	}

	public string GenerateApplicationSecret()
	{
		byte[] key = new byte[32]; // 256 bits
		RandomNumberGenerator.Fill(key);
		return Convert.ToBase64String(key);
	}

	public byte[] GetJwtSigningKey(string key)
	{
		return Convert.FromBase64String(key);
	}
}