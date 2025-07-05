using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookManager.Model;
using BookManager.Services;

namespace BookManager.Views
{
    /// <summary>
    /// Interaction logic for AuthorManagementWindow.xaml
    /// </summary>
    public partial class AuthorManagementWindow : Window
    {
        private readonly BookService _bookService;
        private ObservableCollection<Authors> _authors;

        public AuthorManagementWindow()
        {
            InitializeComponent();
            _bookService = new BookService();
            _authors = new ObservableCollection<Authors>();
            AuthorsDataGrid.ItemsSource = _authors;

            // Wire up events
            AddAuthorButton.Click += AddAuthorButton_Click;
            UpdateAuthorButton.Click += UpdateAuthorButton_Click;
            DeleteAuthorButton.Click += DeleteAuthorButton_Click;
            RefreshButton.Click += RefreshButton_Click;
            CloseButton.Click += CloseButton_Click;

            LoadAuthors();
        }

        private async void LoadAuthors()
        {
            try
            {
                var authors = await _bookService.GetAllAuthorsAsync();
                _authors.Clear();
                foreach (var author in authors)
                {
                    _authors.Add(author);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading authors: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newAuthor = new Authors
                {
                    FirstName = "New",
                    LastName = "Author",
                    Nationality = "",
                    CreatedDate = DateTime.Now
                };

                var createdAuthor = await _bookService.CreateAuthorAsync(newAuthor);
                _authors.Add(createdAuthor);
                AuthorsDataGrid.SelectedItem = createdAuthor;

                MessageBox.Show("Author added successfully!", "Success",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding author: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Authors selectedAuthor)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(selectedAuthor.FirstName) ||
                        string.IsNullOrWhiteSpace(selectedAuthor.LastName))
                    {
                        MessageBox.Show("First name and last name are required.", "Validation Error",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _bookService.UpdateAuthorAsync(selectedAuthor);
                    MessageBox.Show("Author updated successfully!", "Success",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating author: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an author to update.", "No Selection",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void DeleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Authors selectedAuthor)
            {
                try
                {
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete '{selectedAuthor.FullName}'?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _bookService.DeleteAuthorAsync(selectedAuthor.AuthorId);
                        _authors.Remove(selectedAuthor);
                        MessageBox.Show("Author deleted successfully!", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting author: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an author to delete.", "No Selection",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAuthors();
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
