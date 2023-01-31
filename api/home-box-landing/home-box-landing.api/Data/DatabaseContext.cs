using home_box_landing.api.Features.Deploy.Types;
using Microsoft.EntityFrameworkCore;

namespace home_box_landing.api.Data
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;database=home-box-app;username=postgres;password=password";
            
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if(envName == "Production")
                connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            
            optionsBuilder.UseNpgsql(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        
        public DbSet<DeployRecord> Deploys { get; set; }
    }
}