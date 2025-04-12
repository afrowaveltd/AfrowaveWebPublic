// File: Pages/Help/HelpPagesTests.cs
using Newtonsoft.Json;
using System.Reflection;

namespace Id.Tests.Pages.Help
{
	/// <summary>
	/// Tests the validity of help page models by checking their properties and fields. It retrieves help page types from a
	/// configuration file.
	/// </summary>
	public class HelpPagesTests
	{
		private static readonly string ConfigPath = "HelpPagesTestConfig.json";

		/// <summary>
		/// Retrieves help page types from a specified assembly, filtering based on configuration settings. It checks for the
		/// presence of 'LiElements' in each type.
		/// </summary>
		/// <returns>Yields an enumerable collection of objects containing the type and a boolean indicating the presence of
		/// 'LiElements'.</returns>
		public static IEnumerable<object[]> GetHelpPages()
		{
			TestConfig? config = JsonConvert.DeserializeObject<TestConfig>(File.ReadAllText(ConfigPath));
			IEnumerable<Type> helpPageTypes = Assembly.Load("Id")
				 .GetTypes()
				 .Where(t => t.IsSubclassOf(typeof(PageModel)) && t.Namespace == "Id.Pages.Helps")
				 .Where(t => !config.ExcludedPages.Contains(t.Name));

			foreach(Type? type in helpPageTypes)
			{
				bool hasLiElements = type.GetField("LiElements") != null;
				yield return new object[] { type, hasLiElements };
			}
		}

		/// <summary>
		/// Validates the help page model by checking its title and lines, and optionally its list elements.
		/// </summary>
		/// <param name="modelType">Specifies the type of the page model being tested for validation.</param>
		/// <param name="hasLiElements">Indicates whether the model should include a list of elements for additional validation.</param>
		[Theory]
		[MemberData(nameof(GetHelpPages))]
		public void HelpPage_Should_Be_Valid(Type modelType, bool hasLiElements)
		{
			Console.WriteLine($"Testing page model: {modelType.FullName}");

			// Arrange – create mock localizer
			_ = typeof(IStringLocalizer<>).MakeGenericType(modelType);
			object? mockLocalizer = Activator.CreateInstance(typeof(MockLocalizer<>).MakeGenericType(modelType));
			PageModel model = (PageModel)Activator.CreateInstance(modelType, mockLocalizer)!;

			// Act
			_ = (modelType.GetMethod("OnGet")?.Invoke(model, []));

			// Assert Title (check property or field)
			PropertyInfo? titleProp = modelType.GetProperty("Title");
			FieldInfo? titleField = modelType.GetField("Title");
			string? title = titleProp?.GetValue(model) as string ?? titleField?.GetValue(model) as string;
			Assert.False(string.IsNullOrWhiteSpace(title));

			// Assert Lines (check property or field)
			List<string>? lines = modelType.GetProperty("Lines")?.GetValue(model) as List<string>
						 ?? modelType.GetField("Lines")?.GetValue(model) as List<string>;
			Assert.NotNull(lines);
			Assert.NotEmpty(lines);
			Console.WriteLine($"Lines count: {lines?.Count}");

			// Optional LiElements
			if(hasLiElements)
			{
				List<string>? li = modelType.GetField("LiElements")?.GetValue(model) as List<string>;
				Assert.NotNull(li);
				Assert.NotEmpty(li);
				Console.WriteLine($"LiElements count: {li?.Count}");
			}
		}

		private class TestConfig
		{
			[JsonProperty("excludedPages")]
			public List<string> ExcludedPages { get; set; } = new();
		}

		// Mock localizer for testing (returns key as value)
		/// <summary>
		/// A mock implementation of a string localizer for testing purposes that returns the key as the value.
		/// </summary>
		/// <typeparam name="T">Used to specify the type for which the localization is being provided.</typeparam>
		public class MockLocalizer<T> : IStringLocalizer<T>
		{
			/// <summary>
			/// Localized String
			/// </summary>
			/// <param name="name">key to localize</param>
			/// <returns>localized text</returns>
			public LocalizedString this[string name] => new(name, name);

			/// <summary>
			/// Creates a localized string using a specified name and formatting it with provided arguments.
			/// </summary>
			/// <param name="name">Specifies the key for the localized string.</param>
			/// <param name="arguments">Contains values used to format the localized string.</param>
			/// <returns>Returns a new localized string formatted with the given name and arguments.</returns>
			public LocalizedString this[string name, params object[] arguments] => new(name, string.Format(name, arguments));

			/// <summary>
			/// Retrieves all localized strings available in the system.
			/// </summary>
			/// <param name="includeParentCultures">Determines whether to include strings from parent cultures in the result.</param>
			/// <returns>An enumerable collection of localized strings.</returns>
			public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];

			/// <summary>
			/// Returns an instance of the localizer configured for a specific culture.
			/// </summary>
			/// <param name="culture">Specifies the cultural context for localization.</param>
			/// <returns>An instance of the localizer with the specified cultural settings.</returns>
			public IStringLocalizer WithCulture(System.Globalization.CultureInfo culture) => this;
		}
	}
}