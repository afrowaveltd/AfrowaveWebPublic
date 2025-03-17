using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Id.Api;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

/// <summary>
/// Unit tests for the <see cref="IsEmailUnique"/> class.
/// </summary>
public class IsEmailUniqueTests
{
    private readonly Mock<IUsersManager> _usersManagerMock;
    private readonly Mock<IStringLocalizer<IsEmailUnique>> _localizerMock;
    private readonly IsEmailUnique _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="IsEmailUniqueTests"/> class.
    /// </summary>
    public IsEmailUniqueTests()
    {
        _usersManagerMock = new Mock<IUsersManager>();
        _localizerMock = new Mock<IStringLocalizer<IsEmailUnique>>();

        _controller = new IsEmailUnique(_usersManagerMock.Object, _localizerMock.Object);
    }

    /// <summary>
    /// Tests whether OnGetAsync returns true when the email is unique.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task OnGetAsync_ShouldReturnTrue_WhenEmailIsUnique()
    {
        // Arrange
        _usersManagerMock.Setup(m => m.IsEmailFreeAsync("unique@example.com")).ReturnsAsync(true);

        // Act
        var result = await _controller.OnGetAsync("unique@example.com") as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { isUnique = true });

        _usersManagerMock.Verify(m => m.IsEmailFreeAsync("unique@example.com"), Times.Once);
    }

    /// <summary>
    /// Tests whether OnGetAsync returns false when the email is taken.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task OnGetAsync_ShouldReturnFalse_WhenEmailIsTaken()
    {
        // Arrange
        _usersManagerMock.Setup(m => m.IsEmailFreeAsync("taken@example.com")).ReturnsAsync(false);

        // Act
        var result = await _controller.OnGetAsync("taken@example.com") as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { isUnique = false });

        _usersManagerMock.Verify(m => m.IsEmailFreeAsync("taken@example.com"), Times.Once);
    }

    /// <summary>
    /// Tests whether OnGetAsync returns bad request when the email is invalid.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task OnGetAsync_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        _localizerMock.Setup(l => l["Invalid email address"]).Returns(new LocalizedString("Invalid email address", "Invalid email address"));

        // Act
        var result = await _controller.OnGetAsync("invalid-email") as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("Invalid email address");
    }
}
