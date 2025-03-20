namespace Id.Tests.SharedTools.Tests
{
	/// <summary>
	/// Unit tests for the <see cref="EncryptionService"/> class.
	/// These tests validate password hashing, secret generation, JWT key retrieval, and encryption/decryption.
	/// </summary>
	public class EncryptionServiceTests
	{
		/// <summary>
		/// Instance of the encryption service used for testing.
		/// </summary>
		private readonly EncryptionService _encryptionService;

		/// <summary>
		/// Initializes a new instance of the <see cref="EncryptionServiceTests"/> class.
		/// Sets up a mock logger and creates an instance of the encryption service.
		/// </summary>
		public EncryptionServiceTests()
		{
			ILogger<EncryptionService> mockLogger = Substitute.For<ILogger<EncryptionService>>();
			_encryptionService = new EncryptionService(mockLogger);
		}

		/// <summary>
		/// Tests whether a password is hashed correctly and can be successfully verified.
		/// </summary>
		[Fact]
		public void HashPasswordAsync_Should_Create_Hash_And_Verify_Successfully()
		{
			// Arrange
			string password = "SuperSecurePassword123!";

			// Act
			string hashedPassword = _encryptionService.HashPasswordAsync(password);
			bool isVerified = _encryptionService.VerifyPassword(password, hashedPassword);

			// Assert
			Assert.True(isVerified);
		}

		/// <summary>
		/// Tests whether GenerateApplicationSecret returns a valid base64-encoded secret key.
		/// </summary>
		[Fact]
		public void GenerateApplicationSecret_Should_Return_Base64String()
		{
			// Act
			string secret = _encryptionService.GenerateApplicationSecret();

			// Assert
			byte[] secretBytes = Convert.FromBase64String(secret);
			Assert.Equal(32, secretBytes.Length); // Should be a 256-bit key
		}

		/// <summary>
		/// Tests whether GetJwtSigningKey returns the correct byte array from a secret key.
		/// </summary>
		[Fact]
		public void GetJwtSigningKey_Should_Return_Correct_Bytes()
		{
			// Arrange
			string secret = _encryptionService.GenerateApplicationSecret();

			// Act
			byte[] keyBytes = _encryptionService.GetJwtSigningKey(secret);

			// Assert
			Assert.Equal(32, keyBytes.Length);
		}

		/// <summary>
		/// Tests whether EncryptTextAsync correctly encrypts text and DecryptTextAsync restores it.
		/// </summary>
		[Fact]
		public void EncryptTextAsync_And_DecryptTextAsync_Should_Work_Correctly()
		{
			// Arrange
			string text = "Hello, world!";
			string secret = _encryptionService.GenerateApplicationSecret();

			// Act
			string encryptedText = _encryptionService.EncryptTextAsync(text, secret);
			string decryptedText = _encryptionService.DecryptTextAsync(encryptedText, secret);

			// Assert
			Assert.NotEqual(text, encryptedText); // Encryption should modify text
			Assert.Equal(text, decryptedText); // Decryption should restore text
		}
	}
}