using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Services
{
	public class SelectOptionsServices : ISelectOptionsServices
	{
		private readonly ApplicationDbContext _context;
		private readonly IApplicationLoader _loader;

		private readonly string _cssFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
									  .Substring(0, AppDomain.CurrentDomain.BaseDirectory
									  .IndexOf("bin")), "wwwroot", "css");

		public SelectOptionsServices(ApplicationDbContext context, IApplicationLoader loader)
		{
			_context = context;
			_loader = loader;
		}

		public async Task<List<SelectListItem>> GetLanguagesOptionsAsync(string selected)
		{
			string[] codes = _loader.GetSupportedCultures();
			if(codes.Length == 0)
			{
				return new();
			}

			List<SelectListItem> languages = new();

			foreach(string code in codes)
			{
				Language? language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == code);
				if(language != null)
				{
					languages.Add(new SelectListItem
					{
						Selected = language.Code == selected,
						Value = language.Code,
						Text = language.Native
					});
				}
			}

			languages = languages.OrderBy(l => l.Text).ToList();

			return languages;
		}

		public async Task<List<SelectListItem>> GetThemesAsync(string selected)
		{
			List<string> themes = await GetThemeNamesAsync();

			List<SelectListItem> items = [];

			foreach(string theme in themes)
			{
				items.Add(new SelectListItem
				{
					Selected = theme == selected,
					Value = theme,
					Text = theme
				});
			}

			return items;
		}

		public async Task<List<SelectListItem>> GetSecureSocketOptionsAsync(SecureSocketOptions selected = SecureSocketOptions.Auto)
		{
			var options = Enum.GetValues<SecureSocketOptions>();

			var items = options.Select(option => new SelectListItem
			{
				Value = ((int)option).ToString(),
				Text = option.ToString(),
				Selected = option == selected
			}).ToList();

			return await Task.FromResult(items);
		}

		public async Task<string> GetDirectionAsync(string code)
		{
			Language? language = await _context.Languages.FirstOrDefaultAsync(l => l.Code == code);
			if(language == null)
			{
				return "ltr";
			}
			return language.Rtl == 1 ? "rtl" : "ltr";
		}

		private async Task<List<string>> GetThemeNamesAsync()
		{
			if(!Directory.Exists(_cssFolderPath))
			{
				throw new DirectoryNotFoundException($"The folder '{_cssFolderPath}' does not exist.");
			}

			return await Task.Run(() =>
			{
				var themeFiles = Directory.GetFiles(_cssFolderPath, "*-theme.css", SearchOption.TopDirectoryOnly);

				var themeNames = themeFiles
					 .Select(file => Path.GetFileNameWithoutExtension(file)) // Extract file name without extension
					 .Where(fileName => fileName.EndsWith("-theme"))          // Ensure it ends with "-theme"
					 .Select(fileName => fileName.Replace("-theme", ""))     // Remove the "-theme" part
					 .ToList();

				return themeNames;
			});
		}
	}
}