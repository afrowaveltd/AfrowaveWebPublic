using Id.Tests.Data;
using Id.Models.DatabaseModels;
using Xunit;

public class UserTests
{
    [Fact]
    public void Can_Insert_User_Into_Database()
    {
        // Arrange: Create the test database context
        using var context = new ApplicationDbContextTesting();
        context.EnsureDatabaseCreated(); // Ensure schema is created

        // Create a valid User object
        var user = new User
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
        context.Users.Add(user);
        context.SaveChanges();

        // Assert: Verify the user exists in the database
        var retrievedUser = context.Users.Find(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal("testuser@example.com", retrievedUser.Email);
        Assert.Equal("Test User", retrievedUser.DisplayName);
    }
}
