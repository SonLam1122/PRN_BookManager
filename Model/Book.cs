using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Model
{
    public class Book
    {
        public int BookId { get; set; }
        public String Title { get; set; }
        public String ISBN { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int PublishYear { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Authors> Authors { get; set; }
        public virtual ICollection<Categories> Categories { get; set; }
    }
}
