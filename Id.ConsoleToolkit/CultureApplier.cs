using System.Globalization;

public static class CultureApplier
{
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