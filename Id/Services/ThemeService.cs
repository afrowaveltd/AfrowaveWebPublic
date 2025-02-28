namespace Id.Services
{
	/// <summary>
	/// Service to handle themes.
	/// </summary>
	/// <param name="logger">Logger</param>
	/// <param name="environment">WebHostEnvironment</param>
	public class ThemeService(ILogger<ThemeService> logger,
											  IWebHostEnvironment environment) : IThemeService
	{
		private readonly ILogger<ThemeService> _logger = logger;
		private readonly string _cssFolderPath = Path.Combine(environment.WebRootPath, "css");

		/// <summary>
		/// Get the CSS file path.
		/// </summary>
		/// <returns>CSS file path</returns>
		public string GetCssFolderPath() => _cssFolderPath;

		/// <summary>
		/// Get names of all themes.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>List of strings with names of Themes</returns>
		/// <exception cref="DirectoryNotFoundException"></exception>
		public async Task<List<string>> GetThemeNamesAsync(string? userId)
		{
			if(!Directory.Exists(_cssFolderPath))
			{
				throw new DirectoryNotFoundException($"The folder '{_cssFolderPath}' does not exist.");
			}

			return await Task.Run(() =>
			{
				string[] themeFiles = Directory.GetFiles(_cssFolderPath, "*-theme.css", SearchOption.TopDirectoryOnly);

				List<string> themeNames = themeFiles
					 .Select(file => Path.GetFileNameWithoutExtension(file)) // Extract file name without extension
					 .Select(fileName =>
					 {
						 string[] parts = fileName.Split('_', 2); // Split by first underscore
						 return parts.Length == 2 ? (parts[0], parts[1]) : ("public", fileName); // Extract UserId or mark as public
					 })
					 .Where(theme => theme.Item1 == "public" || theme.Item1 == userId) // Filter by UserId or public
					 .Select(theme => theme.Item2.Replace("-theme", "")) // Remove "-theme" part
					 .ToList();

				return themeNames;
			});
		}

		/// <summary>
		/// Ensure that the complete theme files are present.
		/// </summary>
		/// <returns></returns>
		public async Task EnsureCompleteThemeFilesAsync()
		{
			string referenceThemePath = Path.Combine(_cssFolderPath, "light-theme.scss");
			if(!File.Exists(referenceThemePath))
			{
				_logger.LogError("Reference theme file not found.");
				return;
			}
			string referenceContent = await File.ReadAllTextAsync(referenceThemePath);

			// Extract only non-color definitions from reference, keeping the first line (assumed font import)
			string[] referenceLines = referenceContent.Split('\n');
			string referenceFontLine = referenceLines.Length > 0 ? referenceLines[0] : "";
			string nonColorContent = string.Join("\n", referenceLines.Skip(1).Where(line => !line.Trim().StartsWith("$")));

			string[] themeFiles = Directory.GetFiles(_cssFolderPath, "*-theme.scss", SearchOption.TopDirectoryOnly);
			foreach(string themeFile in themeFiles)
			{
				string content = await File.ReadAllTextAsync(themeFile);
				string[] lines = content.Split('\n');

				// Keep the first line (assumed font) from the user's file if it exists
				string userFontLine = lines.Length > 0 ? lines[0] : "";
				string userColorLines = string.Join("\n", lines.Skip(1).Where(line => line.Trim().StartsWith("$")));
				string existingNonColorContent = string.Join("\n", lines.Skip(1).Where(line => !line.Trim().StartsWith("$")));

				// Check if non-color content is different from reference
				if(existingNonColorContent.Trim() != nonColorContent.Trim())
				{
					string mergedContent = $"{userFontLine}\n{userColorLines}\n{nonColorContent}";
					await File.WriteAllTextAsync(themeFile, mergedContent);
				}
			}
		}
	}
}