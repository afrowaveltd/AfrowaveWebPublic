using Microsoft.EntityFrameworkCore;
using Id.Data;

namespace Id.Tests.Data
{
    /// <summary>
    /// Represents the in-memory SQLite database context for testing purposes.
    /// </summary>
    public class ApplicationDbContextTesting : ApplicationDbContext
    {
        public ApplicationDbContextTesting()
            : base(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=:memory:") // In-memory database for testing
                .Options)
        {
        }

        /// <summary>
        /// Configures the database provider and sets migrations assembly for test project.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=:memory:",
                    b => b.MigrationsAssembly("Id.Tests")); // Store test migrations in Id.Tests
            }
        }

        /// <summary>
        /// Ensures the test database is created and remains open.
        /// </summary>
        public void EnsureDatabaseCreated()
        {
            this.Database.OpenConnection(); // Keeps in-memory DB active
            this.Database.EnsureCreated(); // Ensures schema is created
        }
    }
}
