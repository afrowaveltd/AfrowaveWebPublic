using Id.Models.CommunicationModels;

namespace Id.Services
{
   public class UserService : IUserService
   {
      private readonly ApplicationDbContext _context;

      public UserService(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<bool> IsEmailUnique(string email)
      {
         return (!await _context.Users.Where(s => s.Email == email.ToLower().Trim()).AnyAsync());
      }

      // ToDo: Implement CreateUserAsync
      public async Task<ApiResponse<string>> CreateUserAsync(CreateUserModel user)
      {
         ApiResponse<string> response = new ApiResponse<string>();

         return response;
      }

      public async Task<ApiResponse<int>> CreateApplicationUserAsync(CreateApplicationUserModel user)
      {
         ApiResponse<int> response = new ApiResponse<int>();
         if(await _context.ApplicationUsers.Where(s => s.ApplicationId == user.ApplicationId && s.UserId == user.UserId).AnyAsync())
         {
            response.Message = "User already exists";
            response.Successful = true;
            response.Data = await _context.ApplicationUsers
               .Where(s => s.ApplicationId == user.ApplicationId && s.UserId == user.UserId)
               .Select(s => s.Id)
               .FirstOrDefaultAsync();
            return response;
         }
         else
         {
            if(!await _context.Applications.Where(s => s.Id == user.ApplicationId).AnyAsync())
            {
               response.Message = "Application not found";
               response.Successful = false;
               return response;
            }
            if(!await _context.Users.Where(s => s.Id == user.UserId).AnyAsync())
            {
               response.Message = "User not found";
               response.Successful = false;
               return response;
            }

            ApplicationUser applicationUser = new ApplicationUser
            {
               ApplicationId = user.ApplicationId,
               UserId = user.UserId,
               AgreedToCookies = user.AgreedToCookies,
               AgreedSharingUserDetails = user.AgreedSharingUserDetails,
               AgreedToTerms = user.AgreedToTerms,
               Application = await _context.Applications.FindAsync(user.ApplicationId),
               User = await _context.Users.FindAsync(user.UserId)
            };
            _ = await _context.ApplicationUsers.AddAsync(applicationUser);
            _ = await _context.SaveChangesAsync();
            response.Successful = true;
            response.Data = applicationUser.Id;
            return response;
         }
      }

      public async Task<ApiResponse<bool>> DeleteApplicationUserAsync(int id)
      {
         ApiResponse<bool> response = new ApiResponse<bool>();

         return response;
      }

      public async Task<ApiResponse<List<ApplicationUser>>> GetApplicationUsersAsync(string applicationId)
      {
         ApiResponse<List<ApplicationUser>> response = new ApiResponse<List<ApplicationUser>>();
         return response;
      }

      public async Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, CreateUserModel user)
      {
         ApiResponse<bool> response = new ApiResponse<bool>();
         return response;
      }

      public async Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, CreateApplicationUserModel user)
      {
         ApiResponse<bool> response = new ApiResponse<bool>();
         return response;
      }

      public async Task<RoleAssignResult> AsignUserToRoleAsync(string userId, int roleId)
      {
         RoleAssignResult response = new();
         User user = await _context.Users.FindAsync(userId);
         ApplicationRole role = await _context.ApplicationRoles.FindAsync(roleId);

         if(user == null || role == null)
         {
            response.Message = "User or Role not found";
            response.Successful = false;
            return response;
         }

         UserRole userRole = new UserRole
         {
            ApplicationRoleId = roleId,
            UserId = userId,
            User = user,
            ApplicationRole = role
         };
         if(await _context.UserRoles.Where(s => s.UserId == userId && s.ApplicationRoleId == roleId).AnyAsync())
         {
            response.Message = "User already has this role";
            response.Successful = true;
            response.NormalizedName = role.NormalizedName;
            response.RoleId = role.Id;
            response.RoleName = role.Name;
            return response;
         }

         _ = await _context.UserRoles.AddAsync(userRole);
         _ = await _context.SaveChangesAsync();
         response.Successful = true;
         response.UserId = user.Id;
         response.RoleId = role.Id;
         response.RoleName = role.Name;
         response.NormalizedName = role.NormalizedName;
         return response;
      }

      public async Task<ApiResponse<bool>> RemoveUserFromRoleAsync(string userId, int roleId)
      {
         ApiResponse<bool> response = new ApiResponse<bool>();
         UserRole userRole = await _context.UserRoles.Where(s => s.UserId == userId && s.ApplicationRoleId == roleId).FirstOrDefaultAsync();
         if(userRole == null)
         {
            response.Message = "User does not have this role";
            response.Successful = false;
            response.Data = false;
            return response;
         }
         _ = _context.UserRoles.Remove(userRole);
         _ = await _context.SaveChangesAsync();
         response.Successful = true;
         response.Data = true;
         return response;
      }

      public async Task<ApiResponse<List<ApplicationRole>>> GetUserRolesAsync(string userId)
      {
         ApiResponse<List<ApplicationRole>> response = new();
         response.Data = await _context.UserRoles.Where(s => s.UserId == userId).Select(s => s.ApplicationRole).ToListAsync() ?? [];
         return response;
      }

      public async Task<List<RoleAssignResult>> AsignUserToRolesAsync(string userId, List<int> roleIds)
      {
         List<RoleAssignResult> response = [];

         foreach(int roleId in roleIds)
         {
            response.Add(await AsignUserToRoleAsync(userId, roleId));
         }

         return response;
      }

      public async Task<List<RoleAssignResult>> AssignAllRolesToOwner(string userId, string applicationId)
      {
         List<RoleAssignResult> responses = [];
         List<ApplicationRole> roles = await _context.ApplicationRoles.Where(s => s.ApplicationId == applicationId).ToListAsync();
         foreach(ApplicationRole role in roles)
         {
            responses.Add(await AsignUserToRoleAsync(userId, role.Id));
         }
         return responses;
      }
   }
}