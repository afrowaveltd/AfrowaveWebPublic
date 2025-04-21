using System.Text;

namespace Id.Services
{
	public class StylesManager(ApplicationDbContext dbContext,
		IStringLocalizer<StylesManager> t,
		ILogger<StylesManager> logger,
		ISettingsService settingsService)
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly IStringLocalizer<StylesManager> _t = t;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly ILogger<StylesManager> _logger = logger;

		/// <summary>
		/// Determines whether themes are enabled in the application.
		/// </summary>
		/// <remarks>This method asynchronously retrieves the theme setting from the underlying settings
		/// service.</remarks>
		/// <returns><see langword="true"/> if themes are enabled; otherwise, <see langword="false"/>.</returns>
		public async Task<bool> ThemesEnabled()
		{
			return await _settingsService.ThemesEnabled();
		}

		/// <summary>
		/// Asynchronously retrieves the CSS string representation of a theme based on the specified theme ID.
		/// </summary>
		/// <remarks>If themes are disabled or the specified theme ID is 0, the method returns the default theme's CSS
		/// string.</remarks>
		/// <param name="themeId">The unique identifier of the theme to retrieve. A value of 0 indicates the default theme.</param>
		/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains the CSS string
		/// representation of the theme.</returns>
		public async Task<string> GetThemeAsync(int themeId)
		{
			ThemeDefinition theme = new();
			if(themeId == 0 || await ThemesEnabled() == false)
			{
				return CreateCssString(theme);
			}
			// ToDo: Check if the theme exists
			return CreateCssString(theme);
		}

		/// <summary>
		/// Generates a CSS string based on the provided <see cref="ThemeDefinition"/>.
		/// </summary>
		/// <remarks>The generated CSS includes a `:root` block with CSS variables for various theme properties. If
		/// the <see cref="ThemeDefinition.FontLink"/> property is specified, an `@import` statement for the font link is
		/// included at the top of the CSS.</remarks>
		/// <param name="theme">The <see cref="ThemeDefinition"/> containing theme properties to be used for generating the CSS. If a property in
		/// <paramref name="theme"/> is null or empty, a default value will be used.</param>
		/// <returns>A string representing the generated CSS, including custom properties (CSS variables) defined in the <paramref
		/// name="theme"/>.</returns>
		public string CreateCssString(ThemeDefinition theme)
		{
			ThemeDefinition definition = new();
			StringBuilder css = new();
			if(theme.FontLink != null && theme.FontLink != string.Empty)
			{
				css.AppendLine($"@import url('{theme.FontLink}');");
			}
			css.AppendLine(@":root {");
			css.AppendLine($"--bg: {theme.Background ?? definition.Background}");
			css.AppendLine($"--foreground: {theme.Foreground ?? definition.Foreground}");
			css.AppendLine($"--formForeground: {theme.FormForeground ?? definition.FormForeground}");
			css.AppendLine($"--bodyBackground: {theme.BodyBackground ?? definition.BodyBackground}");
			css.AppendLine($"--link: {theme.Link ?? definition.Link}");
			css.AppendLine($"--linkHover: {theme.LinkHover ?? definition.LinkHover}");
			css.AppendLine($"--linkActive: {theme.LinkActive ?? definition.LinkActive}");
			css.AppendLine($"--formBackground: {theme.FormBackground ?? definition.FormBackground}");
			css.AppendLine($"--redBorder: {theme.RedBorder ?? definition.RedBorder}");
			css.AppendLine($"--disabledBackground: {theme.DisabledBackground ?? definition.DisabledBackground}");
			css.AppendLine($"--disabledForeground: {theme.DisabledForeground ?? definition.DisabledForeground}");
			css.AppendLine($"--formControlValidBackground: {theme.FormControlValidBackground ?? definition.FormControlValidBackground}");
			css.AppendLine($"--formControlInvalidBackground: {theme.FormControlInvalidBackground ?? definition.FormControlInvalidBackground}");
			css.AppendLine($"--navbar: {theme.Navbar ?? definition.Navbar}");
			css.AppendLine($"--hr: {theme.Hr ?? definition.Hr}");
			css.AppendLine($"--success: {theme.Success ?? definition.Success}");
			css.AppendLine($"--error: {theme.Error ?? definition.Error}");
			css.AppendLine($"--warning: {theme.Warning ?? definition.Warning}");
			css.AppendLine($"--info: {theme.Info ?? definition.Info}");
			css.AppendLine($"--highlight: {theme.Highlight ?? definition.Highlight}");
			css.AppendLine($"--my: {theme.My ?? definition.My}");
			css.AppendLine($"--family: {theme.Family ?? definition.Family}");
			css.AppendLine($"--admin: {theme.Admin ?? definition.Admin}");
			css.AppendLine($"--borderLight: {theme.BorderLight ?? definition.BorderLight}");
			css.AppendLine($"--shadow: {theme.Shadow ?? definition.Shadow}");
			css.AppendLine($"--modalBackground: {theme.ModalBackground ?? definition.ModalBackground}");
			css.AppendLine($"--font: {theme.Font ?? definition.Font}");
			css.AppendLine(@"}");
			return css.ToString();
		}
	}
}