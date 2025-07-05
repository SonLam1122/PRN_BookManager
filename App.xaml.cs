using System;
using System.Linq;
using System.Windows;
using BookManager.Data;
using BookManager.Model;
using Microsoft.EntityFrameworkCore;

namespace BookManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Initialize database if needed
                using (var context = new BookStoreContext())
                {
                    // Test connection
                    context.Database.CanConnect();

                    // Ensure database is created
                    context.Database.EnsureCreated();

                    // Add some sample data if tables are empty
                    SeedDatabase(context);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}\n\nPlease check your connection string and SQL Server.",
                               "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SeedDatabase(BookStoreContext context)
        {
            try
            {
                // Add sample categories if none exist
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Categories { CategoryName = "Fiction", Description = "Fiction books" },
                        new Categories { CategoryName = "Non-Fiction", Description = "Non-fiction books" },
                        new Categories { CategoryName = "Science", Description = "Science books" },
                        new Categories { CategoryName = "Technology", Description = "Technology books" }
                    );
                }

                // Add sample authors if none exist
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(
                        new Authors { FirstName = "John", LastName = "Doe", Nationality = "American" },
                        new Authors { FirstName = "Jane", LastName = "Smith", Nationality = "British" },
                        new Authors { FirstName = "Robert", LastName = "Johnson", Nationality = "Canadian" }
                    );
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Seeding error: {ex.Message}");
            }
        }
    }
}
