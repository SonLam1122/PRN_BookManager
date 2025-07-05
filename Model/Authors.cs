using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Model
{
    public class Authors
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
        public string FullName => $"{FirstName} {LastName}";
    }
}
