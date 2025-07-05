using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BookManager.Model
{
    public class Books : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _title = "";
        private string _isbn = "";
        private int? _categoryId;
        private int? _authorId;
        private int _publishYear = DateTime.Now.Year;
        private int _pages = 100;
        private decimal _price = 0;
        private int _stockQuantity = 0;
        private string _description = "";

        [Key]
        public int BookId { get; set; }

        public string Title
        {
            get => _title;
            set { _title = value ?? ""; OnPropertyChanged(); }
        }

        public string ISBN
        {
            get => _isbn;
            set { _isbn = value ?? ""; OnPropertyChanged(); }
        }

        public int? CategoryId
        {
            get => _categoryId;
            set { _categoryId = value; OnPropertyChanged(); }
        }

        public int? AuthorId
        {
            get => _authorId;
            set { _authorId = value; OnPropertyChanged(); }
        }

        public int PublishYear
        {
            get => _publishYear;
            set { _publishYear = value; OnPropertyChanged(); }
        }

        public int Pages
        {
            get => _pages;
            set { _pages = value; OnPropertyChanged(); }
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value ?? ""; OnPropertyChanged(); }
        }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Categories Category { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Authors Author { get; set; }

        // Display properties
        public string CategoryName => Category?.CategoryName ?? "No Category";
        public string AuthorName => Author?.FullName ?? "No Author";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // IDataErrorInfo implementation for validation
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Title):
                        if (string.IsNullOrWhiteSpace(Title))
                            return "Title is required";
                        if (Title.Length > 300)
                            return "Title cannot exceed 300 characters";
                        break;

                    case nameof(ISBN):
                        if (!string.IsNullOrWhiteSpace(ISBN) && ISBN.Length > 20)
                            return "ISBN cannot exceed 20 characters";
                        break;

                    case nameof(PublishYear):
                        if (PublishYear < 1000 || PublishYear > 9999)
                            return "Please enter a valid year (1000-9999)";
                        break;

                    case nameof(Pages):
                        if (Pages <= 0)
                            return "Pages must be greater than 0";
                        if (Pages > 10000)
                            return "Pages cannot exceed 10000";
                        break;

                    case nameof(Price):
                        if (Price < 0)
                            return "Price cannot be negative";
                        if (Price > 999999.99m)
                            return "Price cannot exceed 999999.99";
                        break;

                    case nameof(StockQuantity):
                        if (StockQuantity < 0)
                            return "Stock quantity cannot be negative";
                        if (StockQuantity > 99999)
                            return "Stock quantity cannot exceed 99999";
                        break;

                    case nameof(Description):
                        if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 1000)
                            return "Description cannot exceed 1000 characters";
                        break;
                }

                return null;
            }
        }
    }
}
