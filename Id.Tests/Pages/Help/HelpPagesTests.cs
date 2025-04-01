// File: Pages/Help/HelpPagesTests.cs
using Newtonsoft.Json;
using System.Reflection;

namespace Id.Tests.Pages.Help
{
	public class HelpPagesTests
	{
		private static readonly string ConfigPath = "HelpPagesTestConfig.json";

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
		public class MockLocalizer<T> : IStringLocalizer<T>
		{
			public LocalizedString this[string name] => new(name, name);

			public LocalizedString this[string name, params object[] arguments] => new(name, string.Format(name, arguments));

			public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];

			public IStringLocalizer WithCulture(System.Globalization.CultureInfo culture) => this;
		}
	}
}