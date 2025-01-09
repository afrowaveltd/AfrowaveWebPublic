using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Id.Services
{
	public class SelectOptionsServices : ISelectOptionsServices
	{
		private readonly ApplicationDbContext _context;
		private readonly IApplicationLoader _loader;
		private readonly IStringLocalizer<SelectOptionsServices> t;

		private readonly string _cssFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
									  .Substring(0, AppDomain.CurrentDomain.BaseDirectory
									  .IndexOf("bin")), "wwwroot", "css");

		public SelectOptionsServices(ApplicationDbContext context, IApplicationLoader loader, IStringLocalizer<SelectOptionsServices> _t)
		{
			_context = context;
			_loader = loader;
			t = _t;
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

		public async Task<List<SelectListItem>> GetThemesAsync(string selected, string? userId)
		{
			List<string> themes = await GetThemeNamesAsync(userId);

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
			SecureSocketOptions[] options = Enum.GetValues<SecureSocketOptions>();

			List<SelectListItem> items = options.Select(option => new SelectListItem
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

		public async Task<List<SelectListItem>> GetBinaryOptionsAsync(bool selected = true)
		{
			List<SelectListItem> items = new()
			{
				new SelectListItem
				{
					Value = "true",
					Text = t["Yes"],
					Selected = selected
				},
				new SelectListItem
				{
					Value = "false",
					Text = t["No"],
					Selected = !selected
				}
			};
			return await Task.FromResult(items);
		}

		private async Task<List<string>> GetThemeNamesAsync(string? userId)
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
	}
}