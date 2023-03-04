using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace Pollhub.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>()
                .Property(p => p.Options)
                .IsRequired()
                .HasConversion(
                    o => JsonConvert.SerializeObject(o),
                    o => JsonConvert.DeserializeObject<List<Option>>(o)
                );

            modelBuilder.Entity<Poll>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Poll>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Poll>()
                .Property(p => p.Question)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .Property(p => p.Key)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .Property(p => p.VoteIPs)
                .IsRequired(false)
                .HasConversion(
                    o => JsonConvert.SerializeObject(o),
                    o => JsonConvert.DeserializeObject<List<VoteIPs>>(o)
                );

            modelBuilder.Entity<List<string>>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pollhub.db")}");

        }

    }
}
