using System.Globalization;

/// <summary>
/// This class is used to set the culture for the current thread.
/// </summary>
public static class CultureApplier
{
	/// <summary>
	/// Sets the culture for the current thread to the specified language code.
	/// </summary>
	/// <param name="languageCode">Two letters ISO code for the language</param>
	public static void ApplyCulture(string languageCode)
	{
		try
		{
			CultureInfo culture = new(languageCode);

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}
		catch(CultureNotFoundException)
		{
			CultureInfo fallback = new("en");
			CultureInfo.DefaultThreadCurrentCulture = fallback;
			CultureInfo.DefaultThreadCurrentUICulture = fallback;
		}
	}
}