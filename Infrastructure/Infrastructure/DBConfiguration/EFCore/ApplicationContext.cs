using Domain.Entities;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DBConfiguration.EFCore
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            if (!dbContextOptionsBuilder.IsConfigured)
                dbContextOptionsBuilder.UseNpgsql(EnvironmentManager.GetConnectionString());
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User>? User { get; set; }
    }
}