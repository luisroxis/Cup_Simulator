using Microsoft.EntityFrameworkCore;
using SimulatorAPI.Models;

namespace SimulatorAPI.Context
{
  public class SimulatorContext : DbContext
  {
    public SimulatorContext(DbContextOptions<SimulatorContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      optionsBuilder.UseSqlServer(
        configuration.GetConnectionString("ServerConnection")
        );
    }

    public DbSet<Team> Teams { get; set; }
  }
}