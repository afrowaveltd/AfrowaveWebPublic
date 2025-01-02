using System.ComponentModel.DataAnnotations;

namespace Id.Models.DatabaseModels
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        public DateOnly? BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public Gender Gender { get; set; } = Gender.Other;
        public string? EmailConfirmationToken { get; set; } = string.Empty;

        public bool? EmailConfirmed { get; set; } = false;
        public string? PasswordResetToken { get; set; } = string.Empty;
        public DateTime? PasswordResetTokenExpiration { get; set; }
        public DateTime? EmailConfirmationTokenExpiration { get; set; }

        public int AccessFailedCount { get; set; } = 0;
        public List<Application> OwnedApplications { get; set; } = new();
        public List<UserAddress> UserAddresses { get; set; } = new();
        public List<UserRole> UserRoles { get; set; } = new();
        public List<RefreshToken> RefreshTokens { get; set; } = new();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
    }
}