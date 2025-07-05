using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookManager.Model;
using BookManager.Services;

namespace BookManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BookService _bookService;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Authors> _authors;
        private ObservableCollection<Categories> _categories;
        private Book _selectedBook;
        private string _searchText;
        private int _selectedCategoryFilter;
        private int _selectedAuthorFilter;

        public MainViewModel()
        {
            _bookService = new BookService();
            Books = new ObservableCollection<Book>();
            Authors = new ObservableCollection<Authors>();
            Categories = new ObservableCollection<Categories>();

            // Initialize commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            AddBookCommand = new RelayCommand(async () => await AddBookAsync());
            UpdateBookCommand = new RelayCommand(async () => await UpdateBookAsync(), () => SelectedBook != null);
            DeleteBookCommand = new RelayCommand(async () => await DeleteBookAsync(), () => SelectedBook != null);
            SearchCommand = new RelayCommand(async () => await SearchBooksAsync());
            FilterByCategoryCommand = new RelayCommand(async () => await FilterByCategoryAsync());
            FilterByAuthorCommand = new RelayCommand(async () => await FilterByAuthorAsync());
            ClearFiltersCommand = new RelayCommand(async () => await LoadBooksAsync());

            // Load initial data
            _ = LoadDataAsync();
        }

        public ObservableCollection<Book> Books
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

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                ((RelayCommand)UpdateBookCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteBookCommand).RaiseCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public int SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set { _selectedCategoryFilter = value; OnPropertyChanged(); }
        }

        public int SelectedAuthorFilter
        {
            get => _selectedAuthorFilter;
            set { _selectedAuthorFilter = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand UpdateBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand FilterByAuthorCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        private async Task LoadDataAsync()
        {
            await LoadBooksAsync();
            await LoadAuthorsAsync();
            await LoadCategoriesAsync();
        }

        private async Task LoadBooksAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        private async Task LoadAuthorsAsync()
        {
            var authors = await _bookService.GetAllAuthorsAsync();
            Authors.Clear();
            foreach (var author in authors)
            {
                Authors.Add(author);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _bookService.GetAllCategoriesAsync();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task AddBookAsync()
        {
            var newBook = new Book
            {
                Title = "New Book",
                ISBN = "000-0000000000",
                PublishYear = DateTime.Now.Year,
                Pages = 100,
                Price = 0,
                StockQuantity = 0,
                Description = "New book description"
            };

            var createdBook = await _bookService.CreateBookAsync(newBook);
            Books.Add(createdBook);
        }

        private async Task UpdateBookAsync()
        {
            if (SelectedBook != null)
            {
                await _bookService.UpdateBookAsync(SelectedBook);
                await LoadBooksAsync();
            }
        }

        private async Task DeleteBookAsync()
        {
            if (SelectedBook != null)
            {
                await _bookService.DeleteBookAsync(SelectedBook.BookId);
                Books.Remove(SelectedBook);
                SelectedBook = null;
            }
        }

        private async Task SearchBooksAsync()
        {
            var books = await _bookService.SearchBooksAsync(SearchText);
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        private async Task FilterByCategoryAsync()
        {
            if (SelectedCategoryFilter > 0)
            {
                var books = await _bookService.FilterBooksByCategoryAsync(SelectedCategoryFilter);
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
        }

        private async Task FilterByAuthorAsync()
        {
            if (SelectedAuthorFilter > 0)
            {
                var books = await _bookService.FilterBooksByAuthorAsync(SelectedAuthorFilter);
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            await _executeAsync();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
