using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ex1.Models
{
    public class DBContext : DbContext
    {
        public DBContext() { }

        public DbSet<Function> Functions { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<Expression> Expressions { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Expression>().HasOne(e => e.Function).WithMany(f => f.Expressions).HasForeignKey(e => e.FunctionId).IsRequired(false);
        //    modelBuilder.Entity<Expression>().HasOne(e => e.Constant).WithMany(c => c.Expressions).HasForeignKey(e => e.ConstantId).IsRequired(false);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}