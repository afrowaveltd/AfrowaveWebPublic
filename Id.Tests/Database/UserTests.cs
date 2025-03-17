using Id.Tests.Data;

namespace Id.Tests.Database;

public class UserTests
{
	[Fact]
	public void Can_Insert_User_Into_Database()
	{
		// Arrange: Create the test database context
		using ApplicationDbContextTesting context = new ApplicationDbContextTesting();
		context.EnsureDatabaseCreated(); // Ensure schema is created

		// Create a valid User object
		User user = new User
		{
			Email = "testuser@example.com",
			DisplayName = "Test User",
			Firstname = "Test",
			Lastname = "User",
			Password = "SecurePassword123!", // Required field
			PhoneNumber = "+1234567890",
			ProfilePicture = "https://example.com/avatar.png",
			BirthDate = new DateOnly(1990, 5, 15),
			Gender = Gender.Male,
			EmailConfirmed = true,
		};

		// Act: Add and save user
		_ = context.Users.Add(user);
		_ = context.SaveChanges();

		// Assert: Verify the user exists in the database
		User? retrievedUser = context.Users.Find(user.Id);
		Assert.NotNull(retrievedUser);
		Assert.Equal("testuser@example.com", retrievedUser.Email);
		Assert.Equal("Test User", retrievedUser.DisplayName);
	}
}