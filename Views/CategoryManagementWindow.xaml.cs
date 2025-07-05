using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookManager.Model;
using BookManager.Services;

namespace BookManager.Views
{
    public partial class CategoryManagementWindow : Window
    {
        private readonly BookService _bookService;
        private ObservableCollection<Categories> _categories;

        public CategoryManagementWindow()
        {
            InitializeComponent();
            _bookService = new BookService();
            _categories = new ObservableCollection<Categories>();
            CategoriesDataGrid.ItemsSource = _categories;

            // Wire up events
            AddCategoryButton.Click += AddCategoryButton_Click;
            UpdateCategoryButton.Click += UpdateCategoryButton_Click;
            DeleteCategoryButton.Click += DeleteCategoryButton_Click;
            RefreshButton.Click += RefreshButton_Click;
            CloseButton.Click += CloseButton_Click;

            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _bookService.GetAllCategoriesAsync();
                _categories.Clear();
                foreach (var category in categories)
                {
                    _categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newCategory = new Categories
                {
                    CategoryName = "New Category",
                    Description = "",
                    CreatedDate = DateTime.Now
                };

                var createdCategory = await _bookService.CreateCategoryAsync(newCategory);
                _categories.Add(createdCategory);
                CategoriesDataGrid.SelectedItem = createdCategory;

                MessageBox.Show("Category added successfully!", "Success",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Categories selectedCategory)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(selectedCategory.CategoryName))
                    {
                        MessageBox.Show("Category name is required.", "Validation Error",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _bookService.UpdateCategoryAsync(selectedCategory);
                    MessageBox.Show("Category updated successfully!", "Success",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating category: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a category to update.", "No Selection",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Categories selectedCategory)
            {
                try
                {
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete '{selectedCategory.CategoryName}'?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _bookService.DeleteCategoryAsync(selectedCategory.CategoryId);
                        _categories.Remove(selectedCategory);
                        MessageBox.Show("Category deleted successfully!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting category: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.", "No Selection",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _bookService?.Dispose();
            base.OnClosed(e);
        }
    }
}
