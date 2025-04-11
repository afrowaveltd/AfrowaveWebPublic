using Id.Extensions;

namespace Id.Tests.Extensions;

/// <summary>
/// Tests for the EnumExtensions class.
/// </summary>
public class EnumExtensionsTests
{
	/// <summary>
	/// Dummy enum for testing purposes.
	/// </summary>
	public enum DummyEnum
	{
		/// <summary>
		/// Represents the Alpha value.
		/// </summary>
		Alpha,

		/// <summary>
		/// Represents the Beta value.
		/// </summary>
		Beta,

		/// <summary>
		/// Represents the Gamma value.
		/// </summary>
		Gamma
	}

	/// <summary>
	/// Tests the TryParseEnum method for valid enum values.
	/// </summary>
	/// <param name="input">Input string to be parsed</param>
	/// <param name="expected">Expected enum value</param>
	[Theory]
	[InlineData("Alpha", DummyEnum.Alpha)]
	[InlineData("beta", DummyEnum.Beta)] // lower case test
	[InlineData("GAMMA", DummyEnum.Gamma)] // upper case test
	public void TryParseEnum_ShouldReturnTrue_ForValidValues(string input, DummyEnum expected)
	{
		// Act
		bool success = input.TryParseEnum<DummyEnum>(out DummyEnum result);

		// Assert
		Assert.True(success);
		Assert.Equal(expected, result);
	}

	/// <summary>
	/// Tests the TryParseEnum method for invalid enum values.
	/// </summary>
	/// <param name="input">Invalid value</param>
	[Theory]
	[InlineData("Delta")]
	[InlineData("")]
	[InlineData(null)]
	public void TryParseEnum_ShouldReturnFalse_ForInvalidValues(string? input)
	{
		// Act
		bool success = input?.TryParseEnum<DummyEnum>(out _) ?? false;

		// Assert
		Assert.False(success);
	}

	/// <summary>
	/// Tests the ParseEnumOrThrow method for valid enum values.
	/// </summary>
	[Fact]
	public void ParseEnumOrThrow_ShouldReturnCorrectEnum_WhenValid()
	{
		// Act
		DummyEnum result = "Alpha".ParseEnumOrThrow<DummyEnum>();

		// Assert
		Assert.Equal(DummyEnum.Alpha, result);
	}

	/// <summary>
	/// Tests the ParseEnumOrThrow method for invalid enum values.
	/// </summary>
	[Fact]
	public void ParseEnumOrThrow_ShouldThrowException_WhenInvalid()
	{
		// Act & Assert
		ArgumentException ex = Assert.Throws<ArgumentException>(() =>
		{
			_ = "InvalidValue".ParseEnumOrThrow<DummyEnum>();
		});

		Assert.Contains("Neplatná hodnota", ex.Message);
	}
}