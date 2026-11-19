using Microsoft.EntityFrameworkCore;
using MasterFinder.Domain.Entities;

namespace MasterFinder.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Executor> Executors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Execution> Executions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}