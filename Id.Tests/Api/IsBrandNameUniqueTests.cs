using Moq;
using Microsoft.AspNetCore.Mvc;
using Id.Api;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

/// <summary>
/// Unit tests for the <see cref="IsBrandNameUnique"/> class.
/// </summary>
public class IsBrandNameUniqueTests
{
    private readonly Mock<IBrandsManager> _brandsManagerMock;
    private readonly IsBrandNameUnique _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="IsBrandNameUniqueTests"/> class.
    /// </summary>
    public IsBrandNameUniqueTests()
    {
        _brandsManagerMock = new Mock<IBrandsManager>();
        _controller = new IsBrandNameUnique(_brandsManagerMock.Object);
    }

    /// <summary>
    /// Tests whether OnGetAsync returns true when the brand name is unique.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task OnGetAsync_ShouldReturnTrue_WhenBrandNameIsUnique()
    {
        // Arrange
        _brandsManagerMock.Setup(m => m.IsNameUnique("UniqueBrand")).ReturnsAsync(true);

        // Act
        var result = await _controller.OnGetAsync("UniqueBrand") as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { IsUnique = true });

        _brandsManagerMock.Verify(m => m.IsNameUnique("UniqueBrand"), Times.Once);
    }

    /// <summary>
    /// Tests whether OnGetAsync returns false when the brand name is taken.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task OnGetAsync_ShouldReturnFalse_WhenBrandNameIsTaken()
    {
        // Arrange
        _brandsManagerMock.Setup(m => m.IsNameUnique("TakenBrand")).ReturnsAsync(false);

        // Act
        var result = await _controller.OnGetAsync("TakenBrand") as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(new { IsUnique = false });

        _brandsManagerMock.Verify(m => m.IsNameUnique("TakenBrand"), Times.Once);
    }
}
