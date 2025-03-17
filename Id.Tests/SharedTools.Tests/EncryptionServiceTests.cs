namespace Id.Tests.SharedTools.Tests
{
	public class EncryptionServiceTests
	{
		private readonly EncryptionService _encryptionService;

		public EncryptionServiceTests()
		{
			Mock<ILogger<EncryptionService>> mockLogger = new Mock<ILogger<EncryptionService>>();
			_encryptionService = new EncryptionService(mockLogger.Object);
		}

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

		[Fact]
		public void GenerateApplicationSecret_Should_Return_Base64String()
		{
			// Act
			string secret = _encryptionService.GenerateApplicationSecret();

			// Assert
			byte[] secretBytes = Convert.FromBase64String(secret);
			Assert.Equal(32, secretBytes.Length); // Should be 256-bit key
		}

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