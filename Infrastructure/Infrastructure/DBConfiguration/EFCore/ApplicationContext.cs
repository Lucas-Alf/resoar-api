using System.Reflection;
using Domain.Entities;
using Domain.Models;
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
            var path = AppDomain.CurrentDomain.BaseDirectory;
            using (var transaction = this.Database.BeginTransaction())
            {
                var indexes = Directory.GetFiles(Path.Combine(path, "Migrations/Index"));
                foreach (var index in indexes)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(index));
                }

                var triggers = Directory.GetFiles(Path.Combine(path, "Migrations/Triggers"));
                foreach (var trigger in triggers)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(trigger));
                }

                var functions = Directory.GetFiles(Path.Combine(path, "Migrations/Functions"));
                foreach (var function in functions)
                {
                    this.Database.ExecuteSqlRaw(File.ReadAllText(function));
                }

                transaction.Commit();
            }
        }

        public DbSet<User>? User { get; set; }
        public DbSet<Institution>? Institution { get; set; }
        public DbSet<KeyWord>? KeyWord { get; set; }
        public DbSet<KnowledgeArea>? KnowledgeArea { get; set; }
        public DbSet<Research>? Research { get; set; }
        public DbSet<ResearchAdvisor>? ResearchAdvisor { get; set; }
        public DbSet<ResearchAdvisorApproval>? ResearchAdvisorApproval { get; set; }
        public DbSet<ResearchAuthor>? ResearchAuthor { get; set; }
        public DbSet<ResearchKeyWord>? ResearchKeyWord { get; set; }
        public DbSet<ResearchKnowledgeArea>? ResearchKnowledgeArea { get; set; }
        public DbSet<ResearchFullTextModel>? ResearchFullTextModel { get; set; }
        public DbSet<GenericValueModel<int>>? GenericIntModel { get; set; }
    }
}