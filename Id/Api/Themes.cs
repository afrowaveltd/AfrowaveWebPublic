﻿using Microsoft.AspNetCore.Authorization;

namespace Id.Api
{
	/// <summary>
	/// Controller for managing themes.
	/// </summary>
	/// <param name="themeService">The theme service</param>
	/// <param name="logger">Logger</param>
	/// <param name="usersManager">Users manager service</param>
	/// <param name="applicationUsersManager">Application users manager service</param>
	/// <param name="t">Localizer</param>
	[Route("api/[controller]")]
	[ApiController]
	public class Themes(IThemeService themeService,
		ILogger<Themes> logger,
		IUsersManager usersManager,
		IApplicationUsersManager applicationUsersManager,
		IStringLocalizer<Themes> t) : ControllerBase
	{
		private readonly ILogger<Themes> _logger = logger;
		private readonly IThemeService _themeService = themeService;
		private readonly IUsersManager _usersManager = usersManager;
		private readonly IApplicationUsersManager _applicationUsersManager = applicationUsersManager;
		private readonly IStringLocalizer<Themes> _t = t;

		/// <summary>
		/// Get names of all themes.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>List of strings with names of Themes</returns>
		[AllowAnonymous]
		[HttpGet("names")]
		public async Task<IActionResult> GetPublicThemeNamesAsync(string? userId)
		{
			return Ok();
		}
	}
}