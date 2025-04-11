namespace Id.Data
{
	/// <summary>
	/// Represents the database context for the application.
	/// </summary>
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		/// <summary>
		/// Gets or sets the Applications table.
		/// </summary>
		public DbSet<Application> Applications { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationDescriptionTranslations table.
		/// </summary>
		public DbSet<ApplicationDescriptionTranslation> ApplicationDescriptionTranslations { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationPolicies table.
		/// </summary>
		public DbSet<ApplicationPolicy> ApplicationPolicies { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationRoles table.
		/// </summary>
		public DbSet<ApplicationRole> ApplicationRoles { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationSmtpSettings table.
		/// </summary>
		public DbSet<ApplicationSmtpSettings> ApplicationSmtpSettings { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationUsers table.
		/// </summary>
		public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

		/// <summary>
		/// Gets or sets the ApplicationUserDescriptionTranslation table.
		/// </summary>
		public DbSet<ApplicationUserDescriptionTranslation> ApplicationUserDescriptionTranslations { get; set; } = null!;

		/// <summary>
		/// Gets or sets the Brands table.
		/// </summary>
		public DbSet<Brand> Brands { get; set; } = null!;

		/// <summary>
		/// Gets or sets the BrandDescriptionTranslations table.
		/// </summary>
		public DbSet<BrandDescriptionTranslation> BrandDescriptionTranslations { get; set; } = null!;

		/// <summary>
		/// Gets or sets the Countries table.
		/// </summary>
		public DbSet<Country> Countries { get; set; } = null!;

		/// <summary>
		/// Gets or sets the Languages table.
		/// </summary>
		public DbSet<Language> Languages { get; set; } = null!;

		/// <summary>
		/// Gets or sets the PolicyTranslations table.
		/// </summary>
		public DbSet<PolicyTranslation> PolicyTranslations { get; set; } = null!;

		/// <summary>
		/// Gets or sets the RefreshTokens table.
		/// </summary>
		public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

		/// <summary>
		/// Gets or sets the SuspendedApplications table.
		/// </summary>
		public DbSet<SuspendedApplication> SuspendedApplications { get; set; } = null!;

		/// <summary>
		/// Gets or sets the SuspendedUsers table.
		/// </summary>
		public DbSet<SuspendedUser> SuspendedUsers { get; set; } = null!;

		/// <summary>
		/// Gets or sets the UiTranslatorLogs table.
		/// </summary>
		public DbSet<UiTranslatorLog> UiTranslatorLogs { get; set; } = null!;

		/// <summary>
		/// Gets or sets the Users table.
		/// </summary>
		public DbSet<User> Users { get; set; } = null!;

		/// <summary>
		/// Gets or sets the UserAddresses table.
		/// </summary>
		public DbSet<UserAddress> UserAddresses { get; set; } = null!;

		/// <summary>
		/// Gets or sets the UserRoles table.
		/// </summary>
		public DbSet<UserRole> UserRoles { get; set; } = null!;

		/// <summary>
		/// Configures the entity relationships and database constraints.
		/// </summary>
		/// <param name="builder">The model builder used to configure entity relationships.</param>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Configure entity relationships
			_ = builder.Entity<Application>().HasKey(a => a.Id);
			_ = builder.Entity<Application>().HasOne(s => s.Owner).WithMany(u => u.OwnedApplications).HasForeignKey(ar => ar.OwnerId);
			_ = builder.Entity<Application>().HasOne(a => a.Brand).WithMany(b => b.Applications).HasForeignKey(a => a.BrandId);
			_ = builder.Entity<ApplicationRole>().HasKey(a => a.Id);
			_ = builder.Entity<ApplicationPolicy>().HasKey(ap => ap.Id);
			_ = builder.Entity<ApplicationPolicy>().HasOne(ap => ap.Application).WithMany(a => a.Policies).HasForeignKey(ap => ap.ApplicationId);
			_ = builder.Entity<ApplicationRole>().HasOne(ar => ar.Application).WithMany(a => a.Roles).HasForeignKey(ar => ar.ApplicationId);
			_ = builder.Entity<ApplicationSmtpSettings>().HasOne(s => s.Application).WithOne(a => a.SmtpSettings).HasForeignKey<ApplicationSmtpSettings>(s => s.ApplicationId);
			_ = builder.Entity<ApplicationUser>().HasOne(au => au.Application).WithMany(a => a.Users).HasForeignKey(au => au.ApplicationId);
			_ = builder.Entity<ApplicationUser>().HasOne(au => au.User).WithMany(u => u.ApplicationUsers).HasForeignKey(au => au.UserId);
			_ = builder.Entity<Brand>().HasKey(b => b.Id);
			_ = builder.Entity<Brand>().HasOne(b => b.Owner).WithMany(u => u.Brands).HasForeignKey(b => b.OwnerId);
			_ = builder.Entity<Country>().HasKey(c => c.Id);
			_ = builder.Entity<Language>().HasKey(l => l.Id);
			_ = builder.Entity<PolicyTranslation>().HasKey(pt => pt.Id);
			_ = builder.Entity<PolicyTranslation>().HasOne(pt => pt.Policy).WithMany(p => p.Translations).HasForeignKey(pt => pt.PolicyId);
			_ = builder.Entity<PolicyTranslation>().HasOne(pt => pt.Language).WithMany(pt => pt.PolicyTranslations).HasForeignKey(pt => pt.LanguageId);
			_ = builder.Entity<RefreshToken>().HasKey(rt => rt.Id);
			_ = builder.Entity<RefreshToken>().HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId);
			_ = builder.Entity<User>().HasKey(u => u.Id);
			_ = builder.Entity<UserAddress>().HasKey(ua => ua.Id);
			_ = builder.Entity<UserAddress>().HasOne(ua => ua.User).WithMany(u => u.UserAddresses).HasForeignKey(ua => ua.UserId);
			_ = builder.Entity<UserAddress>().HasOne(ua => ua.Country).WithMany(u => u.Addresses).HasForeignKey(ua => ua.CountryId);
			_ = builder.Entity<UserRole>().HasKey(ur => ur.Id);
			_ = builder.Entity<UserRole>().HasOne(ur => ur.ApplicationRole).WithMany(s => s.Roles).HasForeignKey(ur => ur.ApplicationRoleId);
			_ = builder.Entity<UserRole>().HasOne(ur => ur.User).WithMany(a => a.UserRoles).HasForeignKey(ur => ur.ApplicationUserId);

			foreach(Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey? relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			{
				relationship.DeleteBehavior = DeleteBehavior.NoAction;
			}
		}
	}
}