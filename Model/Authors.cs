using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookManager.Model
{
    public class Authors : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _nationality;

        [Key]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
        }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
        }

        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters")]
        public string Nationality
        {
            get => _nationality;
            set { _nationality = value; OnPropertyChanged(); }
        }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Books> Books { get; set; } = new List<Books>();

        // Display property
        public string FullName => $"{FirstName} {LastName}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
