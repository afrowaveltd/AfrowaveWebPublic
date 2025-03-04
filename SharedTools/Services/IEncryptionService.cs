namespace SharedTools.Services
{
	/// <summary>
	/// Interface for the EncryptionService class.
	/// </summary>
	public interface IEncryptionService
	{
		/// <summary>
		/// Decrypts a text using a key.
		/// </summary>
		/// <param name="encryptedText">Encrypted text for decryption</param>
		/// <param name="key">Encryption key</param>
		/// <returns>Decrypted string</returns>
		string DecryptTextAsync(string encryptedText, string key);

		/// <summary>
		/// Encrypts a text using a key.
		/// </summary>
		/// <param name="text">Text to be encrypted</param>
		/// <param name="key">Encryption key</param>
		/// <returns>Encrypted string</returns>
		string EncryptTextAsync(string text, string key);

		/// <summary>
		/// Generates a random application secret.
		/// </summary>
		/// <returns>The generated application secret</returns>
		string GenerateApplicationSecret();

		/// <summary>
		/// Gets the JWT signing key bytes from the key string.
		/// </summary>
		/// <param name="key">Encryption key</param>
		/// <returns>JWT signing key bytes</returns>
		byte[] GetJwtSigningKey(string key);

		/// <summary>
		/// Hashes a password.
		/// </summary>
		/// <param name="password">Password to hash</param>
		/// <returns>Hashed password</returns>

		string HashPasswordAsync(string password);

		/// <summary>
		/// Verifies a password.
		/// </summary>
		/// <param name="password">Password entered by user</param>
		/// <param name="hashedPassword">Hashed password from the database</param>
		/// <returns>True, if pasword entered by user is correct</returns>

		bool VerifyPassword(string password, string hashedPassword);
	}
}