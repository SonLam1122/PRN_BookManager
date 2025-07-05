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
        public DbSet<Books> Books { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public BookStoreContext()
        {
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

                    var connectionString = builder.GetConnectionString("DefaultConnection");

                    // Fallback connection string if appsettings.json is not found
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        connectionString = "Server=SonLam;Database=BookStoreManagement;User Id=sa;Password=123;TrustServerCertificate=true;Encrypt=false;";
                    }

                    optionsBuilder.UseSqlServer(connectionString);
                }
                catch (Exception)
                {
                    // Fallback to default connection string if configuration fails
                    var fallbackConnectionString = "Server=SonLam;Database=BookStoreManagement;User Id=sa;Password=123;TrustServerCertificate=true;Encrypt=false;";
                    optionsBuilder.UseSqlServer(fallbackConnectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Book entity
            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
                entity.Property(e => e.ISBN).HasMaxLength(20);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                // Configure relationships
                entity.HasOne(b => b.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(b => b.Author)
                      .WithMany(a => a.Books)
                      .HasForeignKey(b => b.AuthorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Authors entity
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Nationality).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // Configure Categories entity
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
