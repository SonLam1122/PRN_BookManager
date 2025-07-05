using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookManager.Data
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Categories> Categories { get; set; }
        private readonly Configuration _configuration;

        public BookStoreContext()
        {

        }

        public BookStoreContext(Configuration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var connectionString = builder.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ISBN).HasMaxLength(50);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne<Categories>()
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne<Authors>()
                    .WithOne()
                    .HasForeignKey<Authors>(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Nationality).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
