namespace Id.Tests.Helpers
{
	/// <summary>
	/// A fake ITempDataProvider for testing purposes.
	/// </summary>
	public class FakeTempDataProvider : ITempDataProvider
	{
		/// <summary>
		/// Loads an empty dictionary of temp data.
		/// </summary>
		/// <param name="context">The HttpContext</param>
		/// <returns>Dictionary with TempData</returns>
		public IDictionary<string, object?> LoadTempData(HttpContext context) => new Dictionary<string, object?>();

		/// <summary>
		/// Saves temp data to the context.
		/// </summary>
		/// <param name="context">The HttpContext</param>
		/// <param name="values">Parameters values</param>
		public void SaveTempData(HttpContext context, IDictionary<string, object?> values)
		{ }
	}
}