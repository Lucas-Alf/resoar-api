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

        public void RunDatabaseScripts()
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                var indexes = Directory.GetFiles("../../Infrastructure/Infrastructure/Migrations/Index");
                foreach (var index in indexes)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(index));
                }

                var triggers = Directory.GetFiles("../../Infrastructure/Infrastructure/Migrations/Triggers");
                foreach (var trigger in triggers)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(trigger));
                }

                var functions = Directory.GetFiles("../../Infrastructure/Infrastructure/Migrations/Functions");
                foreach (var function in functions)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(function));
                }

                transaction.Commit();
            }
        }

        public DbSet<User>? User { get; set; }
    }
}