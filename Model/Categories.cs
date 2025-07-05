using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Model
{
    public class Categories
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
