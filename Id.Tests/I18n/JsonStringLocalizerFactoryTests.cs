namespace Id.Tests.I18n;

/// <summary>
/// Unit tests for the <see cref="JsonStringLocalizerFactory"/> class.
/// </summary>
public class JsonStringLocalizerFactoryTests
{
	private readonly Mock<IDistributedCache> _mockCache;
	private readonly JsonStringLocalizerFactory _factory;

	/// <summary>
	/// Initializes a new instance of the <see cref="JsonStringLocalizerFactoryTests"/> class.
	/// </summary>
	public JsonStringLocalizerFactoryTests()
	{
		_mockCache = new Mock<IDistributedCache>();
		_factory = new JsonStringLocalizerFactory(_mockCache.Object);
	}

	/// <summary>
	/// Verifies that the <see cref="JsonStringLocalizerFactory"/> constructor initializes the instance correctly.
	/// </summary>
	[Fact]
	public void Create_WithResourceSource_ReturnsJsonStringLocalizer()
	{
		// Act
		IStringLocalizer localizer = _factory.Create(typeof(JsonStringLocalizerTests));

		// Assert
		Assert.NotNull(localizer);
		_ = Assert.IsType<JsonStringLocalizer>(localizer);
	}

	/// <summary>
	/// Verifies that the <see cref="JsonStringLocalizerFactory"/> constructor initializes the instance correctly.
	/// </summary>
	[Fact]
	public void Create_WithBaseNameAndLocation_ReturnsJsonStringLocalizer()
	{
		// Act
		IStringLocalizer localizer = _factory.Create("Resource", "Location");

		// Assert
		Assert.NotNull(localizer);
		_ = Assert.IsType<JsonStringLocalizer>(localizer);
	}
}