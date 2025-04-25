using System.Text;

namespace Id.Services
{
	/// <summary>
	/// Service to manage styles and themes in the application.
	/// </summary>
	/// <param name="dbContext">ApplicationDbContext</param>
	/// <param name="t">Localizer</param>
	/// <param name="logger">Logger</param>
	/// <param name="settingsService">The settings service</param>
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
				_ = css.AppendLine($"@import url('{theme.FontLink}');");
			}
			_ = css.AppendLine(@":root {");
			_ = css.AppendLine($"--bg: {theme.Background ?? definition.Background}");
			_ = css.AppendLine($"--foreground: {theme.Foreground ?? definition.Foreground}");
			_ = css.AppendLine($"--formForeground: {theme.FormForeground ?? definition.FormForeground}");
			_ = css.AppendLine($"--bodyBackground: {theme.BodyBackground ?? definition.BodyBackground}");
			_ = css.AppendLine($"--link: {theme.Link ?? definition.Link}");
			_ = css.AppendLine($"--linkHover: {theme.LinkHover ?? definition.LinkHover}");
			_ = css.AppendLine($"--linkActive: {theme.LinkActive ?? definition.LinkActive}");
			_ = css.AppendLine($"--formBackground: {theme.FormBackground ?? definition.FormBackground}");
			_ = css.AppendLine($"--redBorder: {theme.RedBorder ?? definition.RedBorder}");
			_ = css.AppendLine($"--disabledBackground: {theme.DisabledBackground ?? definition.DisabledBackground}");
			_ = css.AppendLine($"--disabledForeground: {theme.DisabledForeground ?? definition.DisabledForeground}");
			_ = css.AppendLine($"--formControlValidBackground: {theme.FormControlValidBackground ?? definition.FormControlValidBackground}");
			_ = css.AppendLine($"--formControlInvalidBackground: {theme.FormControlInvalidBackground ?? definition.FormControlInvalidBackground}");
			_ = css.AppendLine($"--navbar: {theme.Navbar ?? definition.Navbar}");
			_ = css.AppendLine($"--hr: {theme.Hr ?? definition.Hr}");
			_ = css.AppendLine($"--success: {theme.Success ?? definition.Success}");
			_ = css.AppendLine($"--error: {theme.Error ?? definition.Error}");
			_ = css.AppendLine($"--warning: {theme.Warning ?? definition.Warning}");
			_ = css.AppendLine($"--info: {theme.Info ?? definition.Info}");
			_ = css.AppendLine($"--highlight: {theme.Highlight ?? definition.Highlight}");
			_ = css.AppendLine($"--my: {theme.My ?? definition.My}");
			_ = css.AppendLine($"--family: {theme.Family ?? definition.Family}");
			_ = css.AppendLine($"--admin: {theme.Admin ?? definition.Admin}");
			_ = css.AppendLine($"--borderLight: {theme.BorderLight ?? definition.BorderLight}");
			_ = css.AppendLine($"--shadow: {theme.Shadow ?? definition.Shadow}");
			_ = css.AppendLine($"--modalBackground: {theme.ModalBackground ?? definition.ModalBackground}");
			_ = css.AppendLine($"--font: {theme.Font ?? definition.Font}");
			_ = css.AppendLine(@"}");
			return css.ToString();
		}
	}
}