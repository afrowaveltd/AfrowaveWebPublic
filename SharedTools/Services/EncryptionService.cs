using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace SharedTools.Services
{
	/// <summary>
	/// The EncryptionService class is used to hash passwords, verify passwords, generate application secrets, and get JWT signing keys.
	/// </summary>
	/// <param name="logger">Logger service</param>
	public class EncryptionService(ILogger<EncryptionService> logger) : IEncryptionService
	{
		private readonly ILogger<EncryptionService> _logger = logger;

		private readonly string _settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			[..AppDomain.CurrentDomain.BaseDirectory
				  .IndexOf("bin")], "Settings", "settings.bin");

		/// <summary>
		/// Hashes a password using a salt and the SHA256 algorithm.
		/// </summary>
		/// <param name="password">Password string to be hashed</param>
		/// <returns>String with the password hash</returns>
		public string HashPasswordAsync(string password)
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

		/// <summary>
		/// Verifies a password by comparing the hashed password with the password.
		/// </summary>
		/// <param name="password">Original password string</param>
		/// <param name="hashedPassword">Hashed password string</param>
		/// <returns>true if passwords match</returns>
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

		/// <summary>
		/// Generates a random application secret.
		/// </summary>
		/// <returns>Application secret key as string</returns>
		public string GenerateApplicationSecret()
		{
			using var rng = RandomNumberGenerator.Create();
			byte[] key = new byte[32]; // 256 bits
			rng.GetBytes(key);
			return Convert.ToBase64String(key);
		}

		/// <summary>
		/// Gets the JWT signing key.
		/// </summary>
		/// <param name="key">String with the application secret</param>
		/// <returns>Bytes array JwtSigningKey</returns>
		public byte[] GetJwtSigningKey(string key)
		{
			return Convert.FromBase64String(key);
		}

		/// <summary>
		/// Encrypts the given text using AES encryption with the provided key.
		/// </summary>
		/// <param name="text">The text to encrypt.</param>
		/// <param name="key">The base64-encoded key.</param>
		/// <returns>A base64-encoded encrypted string.</returns>
		public string EncryptTextAsync(string text, string key)
		{
			byte[] keyBytes = Convert.FromBase64String(key);
			byte[] textBytes = Encoding.UTF8.GetBytes(text);

			using Aes aes = Aes.Create();
			aes.Key = keyBytes;
			aes.Mode = CipherMode.CBC; // Cipher Block Chaining mode
			aes.Padding = PaddingMode.PKCS7;

			// Generate a random IV (Initialization Vector)
			aes.GenerateIV();
			byte[] iv = aes.IV;

			using var encryptor = aes.CreateEncryptor();
			byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);

			// Combine IV and encrypted data into a single array
			byte[] result = new byte[iv.Length + encryptedBytes.Length];
			Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
			Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

			return Convert.ToBase64String(result);
		}

		/// <summary>
		/// Decrypts the given encrypted text using AES decryption with the provided key.
		/// </summary>
		/// <param name="encryptedText">The base64-encoded encrypted text.</param>
		/// <param name="key">The base64-encoded key.</param>
		/// <returns>The decrypted text.</returns>
		public string DecryptTextAsync(string encryptedText, string key)
		{
			byte[] keyBytes = Convert.FromBase64String(key);
			byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

			using Aes aes = Aes.Create();
			aes.Key = keyBytes;
			aes.Mode = CipherMode.CBC; // Cipher Block Chaining mode
			aes.Padding = PaddingMode.PKCS7;

			// Extract the IV from the beginning of the encrypted data
			byte[] iv = new byte[aes.BlockSize / 8];
			byte[] cipherText = new byte[encryptedBytes.Length - iv.Length];
			Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
			Buffer.BlockCopy(encryptedBytes, iv.Length, cipherText, 0, cipherText.Length);

			aes.IV = iv;

			using var decryptor = aes.CreateDecryptor();
			byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
			return Encoding.UTF8.GetString(decryptedBytes);
		}
	}
}