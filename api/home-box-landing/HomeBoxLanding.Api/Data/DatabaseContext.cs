using HomeBoxLanding.Api.Features.Deploy.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<DeployRecord> Deploys { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;database=home_app;username=postgres;password=password";
            
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if(envName == "Production")
                connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            
            optionsBuilder.UseNpgsql(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}