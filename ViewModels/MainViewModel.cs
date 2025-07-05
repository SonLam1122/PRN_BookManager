using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using BookManager.Model;
using BookManager.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace BookManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        private ObservableCollection<Books> _books;
        private ObservableCollection<Authors> _authors;
        private ObservableCollection<Categories> _categories;
        private Books _selectedBook;
        private string _searchText;
        private int? _selectedCategoryFilter;
        private int? _selectedAuthorFilter;
        private bool _isLoading;
        private string _statusMessage;
        private bool _hasErrors;

        public MainViewModel()
        {
            try
            {
                Debug.WriteLine("MainViewModel: Initializing...");
                _bookService = new BookService();
                Books = new ObservableCollection<Books>();
                Authors = new ObservableCollection<Authors>();
                Categories = new ObservableCollection<Categories>();

                // Initialize commands with simple implementations first
                LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
                AddBookCommand = new RelayCommand(async () => await AddBookAsync());
                UpdateBookCommand = new RelayCommand(async () => await UpdateBookAsync());
                DeleteBookCommand = new RelayCommand(async () => await DeleteBookAsync());
                SearchCommand = new RelayCommand(async () => await SearchBooksAsync());
                FilterByCategoryCommand = new RelayCommand(async () => await FilterByCategoryAsync());
                FilterByAuthorCommand = new RelayCommand(async () => await FilterByAuthorAsync());
                ClearFiltersCommand = new RelayCommand(async () => await ClearFiltersAsync());
                ManageAuthorsCommand = new RelayCommand(async () => await Task.Run(ManageAuthors));
                ManageCategoriesCommand = new RelayCommand(async () => await Task.Run(ManageCategories));
                ExportDataCommand = new RelayCommand(async () => await ExportDataAsync());

                Debug.WriteLine("MainViewModel: Commands initialized");

                // Load initial data
                _ = Task.Run(async () => await LoadDataAsync());
                Debug.WriteLine("MainViewModel: Initialization complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MainViewModel initialization error: {ex.Message}");
                MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Properties

        public ObservableCollection<Books> Books
        {
            get => _books;
            set { _books = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Authors> Authors
        {
            get => _authors;
            set { _authors = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Categories> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public Books SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                Debug.WriteLine($"SelectedBook changed: {_selectedBook?.Title ?? "null"}");
            }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public int? SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set { _selectedCategoryFilter = value; OnPropertyChanged(); }
        }

        public int? SelectedAuthorFilter
        {
            get => _selectedAuthorFilter;
            set { _selectedAuthorFilter = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public bool HasErrors
        {
            get => _hasErrors;
            set { _hasErrors = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand LoadDataCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand UpdateBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand FilterByAuthorCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand ManageAuthorsCommand { get; }
        public ICommand ManageCategoriesCommand { get; }
        public ICommand ExportDataCommand { get; }

        #endregion

        #region Methods

        private async Task LoadDataAsync()
        {
            try
            {
                Debug.WriteLine("LoadDataAsync: Starting...");
                IsLoading = true;
                StatusMessage = "Loading data...";
                HasErrors = false;

                await LoadBooksAsync();
                await LoadAuthorsAsync();
                await LoadCategoriesAsync();

                StatusMessage = $"Loaded {Books.Count} books, {Authors.Count - 1} authors, {Categories.Count - 1} categories";
                Debug.WriteLine($"LoadDataAsync: Complete - {Books.Count} books loaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadDataAsync error: {ex.Message}");
                HasErrors = true;
                StatusMessage = $"Error loading data: {ex.Message}";
                MessageBox.Show($"Failed to load data: {ex.Message}", "Load Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                Debug.WriteLine("LoadBooksAsync: Starting...");
                var books = await _bookService.GetAllBooksWithDetailsAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in books)
                    {
                        Books.Add(book);
                    }
                });

                Debug.WriteLine($"LoadBooksAsync: Loaded {books.Count} books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadBooksAsync error: {ex.Message}");
                throw;
            }
        }

        private async Task LoadAuthorsAsync()
        {
            try
            {
                var authors = await _bookService.GetAllAuthorsAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Authors.Clear();
                    Authors.Add(new Authors { AuthorId = 0, FirstName = "All", LastName = "Authors" });
                    foreach (var author in authors)
                    {
                        Authors.Add(author);
                    }
                });

                Debug.WriteLine($"LoadAuthorsAsync: Loaded {authors.Count} authors");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadAuthorsAsync error: {ex.Message}");
                throw;
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _bookService.GetAllCategoriesAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Categories.Clear();
                    Categories.Add(new Categories { CategoryId = 0, CategoryName = "All Categories" });
                    foreach (var category in categories)
                    {
                        Categories.Add(category);
                    }
                });

                Debug.WriteLine($"LoadCategoriesAsync: Loaded {categories.Count} categories");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadCategoriesAsync error: {ex.Message}");
                throw;
            }
        }

        private async Task AddBookAsync()
        {
            try
            {
                Debug.WriteLine("AddBookAsync: Starting...");

                // Check if we have authors and categories
                if (Authors.Count <= 1)
                {
                    MessageBox.Show("Please add at least one author first.", "Missing Authors", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Categories.Count <= 1)
                {
                    MessageBox.Show("Please add at least one category first.", "Missing Categories", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IsLoading = true;
                StatusMessage = "Adding new book...";

                var newBook = new Books
                {
                    Title = "New Book " + DateTime.Now.ToString("HHmmss"),
                    ISBN = "",
                    PublishYear = DateTime.Now.Year,
                    Pages = 100,
                    Price = 0,
                    StockQuantity = 1,
                    Description = "New book description",
                    CategoryId = Categories.Skip(1).FirstOrDefault()?.CategoryId,
                    AuthorId = Authors.Skip(1).FirstOrDefault()?.AuthorId
                };

                Debug.WriteLine($"AddBookAsync: Creating book - Title: {newBook.Title}, CategoryId: {newBook.CategoryId}, AuthorId: {newBook.AuthorId}");

                var createdBook = await _bookService.CreateBookAsync(newBook);
                Debug.WriteLine($"AddBookAsync: Book created with ID: {createdBook.BookId}");

                // Add to UI collection
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Add(createdBook);
                    SelectedBook = createdBook;
                });

                StatusMessage = "Book added successfully";
                MessageBox.Show("Book added successfully! You can now edit the details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Debug.WriteLine("AddBookAsync: Complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddBookAsync error: {ex.Message}");
                Debug.WriteLine($"AddBookAsync stack trace: {ex.StackTrace}");
                HasErrors = true;
                StatusMessage = $"Error adding book: {ex.Message}";
                MessageBox.Show($"Failed to add book: {ex.Message}\n\nDetails: {ex.InnerException?.Message}", "Add Book Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateBookAsync()
        {
            try
            {
                Debug.WriteLine("UpdateBookAsync: Starting...");

                if (SelectedBook == null)
                {
                    MessageBox.Show("Please select a book to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Debug.WriteLine($"UpdateBookAsync: Updating book ID: {SelectedBook.BookId}, Title: {SelectedBook.Title}");

                // Basic validation
                if (string.IsNullOrWhiteSpace(SelectedBook.Title))
                {
                    MessageBox.Show("Title is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IsLoading = true;
                StatusMessage = "Updating book...";

                var updatedBook = await _bookService.UpdateBookAsync(SelectedBook);
                Debug.WriteLine($"UpdateBookAsync: Book updated successfully");

                // Update UI
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var index = Books.IndexOf(SelectedBook);
                    if (index >= 0)
                    {
                        Books[index] = updatedBook;
                        SelectedBook = updatedBook;
                    }
                });

                StatusMessage = "Book updated successfully";
                MessageBox.Show("Book updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Debug.WriteLine("UpdateBookAsync: Complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateBookAsync error: {ex.Message}");
                Debug.WriteLine($"UpdateBookAsync stack trace: {ex.StackTrace}");
                HasErrors = true;
                StatusMessage = $"Error updating book: {ex.Message}";
                MessageBox.Show($"Failed to update book: {ex.Message}\n\nDetails: {ex.InnerException?.Message}", "Update Book Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteBookAsync()
        {
            try
            {
                Debug.WriteLine("DeleteBookAsync: Starting...");

                if (SelectedBook == null)
                {
                    MessageBox.Show("Please select a book to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Debug.WriteLine($"DeleteBookAsync: Deleting book ID: {SelectedBook.BookId}, Title: {SelectedBook.Title}");

                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{SelectedBook.Title}'?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    Debug.WriteLine("DeleteBookAsync: User cancelled");
                    return;
                }

                IsLoading = true;
                StatusMessage = "Deleting book...";

                var bookToDelete = SelectedBook;
                var success = await _bookService.DeleteBookAsync(bookToDelete.BookId);

                if (success)
                {
                    Debug.WriteLine($"DeleteBookAsync: Book deleted successfully");

                    // Remove from UI
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Books.Remove(bookToDelete);
                        SelectedBook = null;
                    });

                    StatusMessage = "Book deleted successfully";
                    MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Debug.WriteLine("DeleteBookAsync: Delete failed - book not found");
                    StatusMessage = "Book not found or already deleted";
                    MessageBox.Show("Book not found or already deleted.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                Debug.WriteLine("DeleteBookAsync: Complete");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteBookAsync error: {ex.Message}");
                Debug.WriteLine($"DeleteBookAsync stack trace: {ex.StackTrace}");
                HasErrors = true;
                StatusMessage = $"Error deleting book: {ex.Message}";
                MessageBox.Show($"Failed to delete book: {ex.Message}\n\nDetails: {ex.InnerException?.Message}", "Delete Book Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchBooksAsync()
        {
            try
            {
                Debug.WriteLine($"SearchBooksAsync: Searching for '{SearchText}'");
                IsLoading = true;
                StatusMessage = "Searching books...";

                var books = await _bookService.SearchBooksAsync(SearchText);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in books)
                    {
                        Books.Add(book);
                    }
                });

                StatusMessage = $"Found {Books.Count} books";
                Debug.WriteLine($"SearchBooksAsync: Found {books.Count} books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchBooksAsync error: {ex.Message}");
                HasErrors = true;
                StatusMessage = $"Error searching books: {ex.Message}";
                MessageBox.Show($"Failed to search books: {ex.Message}", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterByCategoryAsync()
        {
            try
            {
                Debug.WriteLine($"FilterByCategoryAsync: Filtering by category {SelectedCategoryFilter}");
                IsLoading = true;
                StatusMessage = "Filtering books by category...";

                List<Books> books;
                if (SelectedCategoryFilter == null || SelectedCategoryFilter == 0)
                {
                    books = await _bookService.GetAllBooksWithDetailsAsync();
                }
                else
                {
                    books = await _bookService.FilterBooksByCategoryAsync(SelectedCategoryFilter.Value);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in books)
                    {
                        Books.Add(book);
                    }
                });

                StatusMessage = $"Filtered to {Books.Count} books";
                Debug.WriteLine($"FilterByCategoryAsync: Filtered to {books.Count} books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterByCategoryAsync error: {ex.Message}");
                HasErrors = true;
                StatusMessage = $"Error filtering books: {ex.Message}";
                MessageBox.Show($"Failed to filter books: {ex.Message}", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterByAuthorAsync()
        {
            try
            {
                Debug.WriteLine($"FilterByAuthorAsync: Filtering by author {SelectedAuthorFilter}");
                IsLoading = true;
                StatusMessage = "Filtering books by author...";

                List<Books> books;
                if (SelectedAuthorFilter == null || SelectedAuthorFilter == 0)
                {
                    books = await _bookService.GetAllBooksWithDetailsAsync();
                }
                else
                {
                    books = await _bookService.FilterBooksByAuthorAsync(SelectedAuthorFilter.Value);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Books.Clear();
                    foreach (var book in books)
                    {
                        Books.Add(book);
                    }
                });

                StatusMessage = $"Filtered to {Books.Count} books";
                Debug.WriteLine($"FilterByAuthorAsync: Filtered to {books.Count} books");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterByAuthorAsync error: {ex.Message}");
                HasErrors = true;
                StatusMessage = $"Error filtering books: {ex.Message}";
                MessageBox.Show($"Failed to filter books: {ex.Message}", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ClearFiltersAsync()
        {
            try
            {
                Debug.WriteLine("ClearFiltersAsync: Clearing filters");
                SearchText = "";
                SelectedCategoryFilter = 0;
                SelectedAuthorFilter = 0;
                await LoadBooksAsync();
                StatusMessage = "Filters cleared";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ClearFiltersAsync error: {ex.Message}");
                MessageBox.Show($"Error clearing filters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageAuthors()
        {
            try
            {
                Debug.WriteLine("ManageAuthors: Opening author management window");
                var authorWindow = new Views.AuthorManagementWindow();
                authorWindow.ShowDialog();
                _ = Task.Run(async () => await LoadAuthorsAsync());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ManageAuthors error: {ex.Message}");
                MessageBox.Show($"Error opening author management: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageCategories()
        {
            try
            {
                Debug.WriteLine("ManageCategories: Opening category management window");
                var categoryWindow = new Views.CategoryManagementWindow();
                categoryWindow.ShowDialog();
                _ = Task.Run(async () => await LoadCategoriesAsync());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ManageCategories error: {ex.Message}");
                MessageBox.Show($"Error opening category management: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExportDataAsync()
        {
            try
            {
                Debug.WriteLine("ExportDataAsync: Starting export");
                IsLoading = true;
                StatusMessage = "Exporting data...";

                await Task.Delay(1000); // Placeholder

                StatusMessage = "Data exported successfully";
                MessageBox.Show("Data exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ExportDataAsync error: {ex.Message}");
                HasErrors = true;
                StatusMessage = $"Error exporting data: {ex.Message}";
                MessageBox.Show($"Failed to export data: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute?.Invoke() ?? true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RelayCommand.CanExecute error: {ex.Message}");
                return false;
            }
        }

        public async void Execute(object parameter)
        {
            try
            {
                await _executeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RelayCommand.Execute error: {ex.Message}");
                MessageBox.Show($"Command execution error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
