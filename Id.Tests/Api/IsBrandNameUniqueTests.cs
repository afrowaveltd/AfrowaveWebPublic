using Microsoft.AspNetCore.Mvc;

namespace Id.Tests.Api
{
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
			_ = _brandsManagerMock.Setup(m => m.IsNameUnique("UniqueBrand")).ReturnsAsync(true);

			// Act
			OkObjectResult? result = await _controller.OnGetAsync("UniqueBrand") as OkObjectResult;

			// Assert
			_ = result.Should().NotBeNull();
			_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = true });

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
			_ = _brandsManagerMock.Setup(m => m.IsNameUnique("TakenBrand")).ReturnsAsync(false);

			// Act
			OkObjectResult? result = await _controller.OnGetAsync("TakenBrand") as OkObjectResult;

			// Assert
			_ = result.Should().NotBeNull();
			_ = result!.Value.Should().BeEquivalentTo(new { IsUnique = false });

			_brandsManagerMock.Verify(m => m.IsNameUnique("TakenBrand"), Times.Once);
		}
	}
}