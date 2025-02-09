using Id.Models.CommunicationModels;

namespace Id.Services
{
	public class RoleService(IStringLocalizer<RoleService> _t,
										ILogger<RoleService> logger,
										ApplicationDbContext context) : IRoleService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly ILogger<RoleService> _logger = logger;
		private readonly IStringLocalizer<RoleService> t = _t;

		public async Task<bool> AssignDefaultRolesToNewUserAsync(string applicationId, string userId)
		{
			if(applicationId == null || userId == null)
			{
				return false;
			}

			List<ApplicationRole> roles = await _context.ApplicationRoles.Where(x => x.ApplicationId == applicationId && x.DefaultForNewUsers).ToListAsync();
			if(roles == null)
			{
				return false;
			}
			if(roles.Count == 0)
			{
				return true;
			}
			foreach(ApplicationRole role in roles)
			{
				_ = _context.UserRoles.Add(new UserRole
				{
					ApplicationRoleId = role.Id,
					UserId = userId
				});
				_logger.LogInformation("Role {roleName} assigned to user {userId}", role.Name, userId);
			}
			try
			{
				_ = await _context.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				return false;
			}
		}

		public async Task<ApiResponse<int>> CreateApplicationRole(CreateApplicationRoleModel role)
		{
			ApiResponse<int> response = new ApiResponse<int>();
			role.Name = role.Name.Trim();
			if(string.IsNullOrWhiteSpace(role.ApplicationId))
			{
				response.Message = t["ApplicationId is required"];
				response.Successful = false;
				return response;
			}

			if(string.IsNullOrWhiteSpace(role.Name))
			{
				response.Message = t["Name is required"];
				response.Successful = false;
				return response;
			}
			if(_context.ApplicationRoles.Any(x => x.ApplicationId == role.ApplicationId && x.Name == role.Name))
			{
				response.Message = t["Role already exists"];
				response.Successful = true;
				response.Data = await _context.ApplicationRoles.Where(s => s.Name == role.Name).Select(s => s.Id).FirstOrDefaultAsync();
			}
			ApplicationRole applicationRole = new ApplicationRole
			{
				ApplicationId = role.ApplicationId,
				Name = role.Name,
				NormalizedName = role.Name.ToUpper(),
				DefaultForNewUsers = role.DefaultForNewUsers,
				CanAsignOrRemoveRoles = role.CanAsignOrRemoveRoles,
				Application = await _context.Applications.FindAsync(role.ApplicationId),
				IsEnabled = true
			};

			if(applicationRole == null || applicationRole.Application == null)
			{
				response.Message = t["Application not found"];
				response.Successful = false;
				return response;
			}

			_ = _context.ApplicationRoles.Add(applicationRole);
			_ = await _context.SaveChangesAsync();
			response.Successful = true;
			response.Data = applicationRole.Id;
			return response;
		}

		public async Task<ApiResponse<bool>> UpdateApplicationRole(int id, CreateApplicationRoleModel role)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();
			ApplicationRole applicationRole = await _context.ApplicationRoles.FindAsync(id);
			if(applicationRole == null)
			{
				response.Message = t["Role not found"];
				response.Successful = false;
				return response;
			}
			if(string.IsNullOrWhiteSpace(role.Name))
			{
				response.Message = t["Role name is required"];
				response.Successful = false;
				return response;
			}
			applicationRole.Name = role.Name;
			applicationRole.DefaultForNewUsers = role.DefaultForNewUsers;
			applicationRole.CanAsignOrRemoveRoles = role.CanAsignOrRemoveRoles;
			_ = _context.ApplicationRoles.Update(applicationRole);
			_ = await _context.SaveChangesAsync();
			response.Successful = true;
			return response;
		}

		public async Task<ApiResponse<bool>> DeleteApplicationRole(int id)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();
			ApplicationRole applicationRole = await _context.ApplicationRoles.FindAsync(id);
			if(applicationRole == null)
			{
				response.Message = t["Role not found"];
				response.Successful = false;
				return response;
			}
			applicationRole.IsEnabled = false;
			_ = await _context.SaveChangesAsync();
			response.Successful = true;
			return response;
		}

		public async Task<ApiResponse<List<ApplicationRole>>> GetApplicationRoles(string applicationId)
		{
			ApiResponse<List<ApplicationRole>> response = new ApiResponse<List<ApplicationRole>>();
			response.Data = await _context.ApplicationRoles.Where(x => x.ApplicationId == applicationId && x.IsEnabled).ToListAsync();
			response.Successful = true;
			return response;
		}

		public async Task<ApiResponse<List<ApplicationRole>>> CreateDefaultRoles(string applicationId)
		{
			ApiResponse<List<ApplicationRole>> response = new ApiResponse<List<ApplicationRole>>();
			Application application = await _context.Applications.FindAsync(applicationId);
			List<ApplicationRole> roles = new List<ApplicationRole>
				{
					 new ApplicationRole
					 {
						ApplicationId = applicationId,
						Name = "Owner",
						NormalizedName = "OWNER",
						DefaultForNewUsers = false,
						CanAsignOrRemoveRoles = true,
						IsEnabled = true,
						Application = application
					 },
					 new ApplicationRole
					 {
						  ApplicationId = applicationId,
						  Name = "Administrator",
						  NormalizedName = "ADMINISTRATOR",
						  DefaultForNewUsers = false,
						  CanAsignOrRemoveRoles = true,
						  IsEnabled = true,
						  Application = application
					 },
					 new ApplicationRole
					 {
						  ApplicationId = applicationId,
						  Name = "Translator",
						  NormalizedName = "TRANSLATOR",
						  DefaultForNewUsers = false,
						  CanAsignOrRemoveRoles = false,
						  IsEnabled = true,
						  Application = application
					 },
					 new ApplicationRole
					 {
						  ApplicationId = applicationId,
						  Name = "User",
						  NormalizedName = "USER",
						  DefaultForNewUsers = true,
						  CanAsignOrRemoveRoles = false,
						  IsEnabled = true,
						  Application = application
					 }
				};
			try
			{
				foreach(ApplicationRole role in roles)
				{
					if(!await _context.ApplicationRoles.AnyAsync(s => s.NormalizedName == role.NormalizedName))
					{
						await _context.ApplicationRoles.AddAsync(role);
						await _context.SaveChangesAsync();
					}
				}

				ApiResponse<List<ApplicationRole>> updatedRoles = await GetApplicationRoles(applicationId);
				response.Data = updatedRoles.Data;
				response.Successful = true;
				return response;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				response.Message = t["An error occurred"];
				response.Successful = false;
				return response;
			}
		}
	}
}