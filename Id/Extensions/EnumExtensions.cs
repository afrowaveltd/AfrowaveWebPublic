namespace Id.Extensions;

/// <summary>
/// Extensions for parsing enums.
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// Parses a string to an enum value.
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	/// <param name="value">Value to be passed as enum</param>
	/// <param name="result">Enum to be used</param>
	/// <returns>Enum value</returns>
	public static bool TryParseEnum<TEnum>(this string value, out TEnum result) where TEnum : struct, Enum
	{
		return Enum.TryParse(value, true, out result);
	}

	/// <summary>
	/// Parses a string to an enum value. If the parsing fails, it throws an exception.
	/// </summary>
	/// <typeparam name="TEnum">Enum to be parsed</typeparam>
	/// <param name="value">value to be parsed</param>
	/// <returns>Enum value</returns>
	/// <exception cref="ArgumentException"></exception>
	public static TEnum ParseEnumOrThrow<TEnum>(this string value) where TEnum : struct, Enum
	{
		if(Enum.TryParse(value, true, out TEnum result))
		{
			return result;
		}

		throw new ArgumentException($"Neplatná hodnota '{value}' pro enum typu {typeof(TEnum).Name}");
	}
}