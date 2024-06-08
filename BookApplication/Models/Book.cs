using System.ComponentModel.DataAnnotations;

namespace BookApplication.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsBorrowed { get; set; }
        public bool IsDeleted { get; set; }
        public List<Author> Authors { get; set; }
    }

    public class BookCreateViewModel
    {
        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Url(ErrorMessage = "The Image URL is not valid.")]
        public string ImageUrl { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        public List<int> AuthorIds { get; set; } = new List<int>();
    }

    public class BookEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Url(ErrorMessage = "The Image URL is not valid.")]
        public string ImageUrl { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        public List<int> AuthorIds { get; set; } = new List<int>(); 
    }

}
