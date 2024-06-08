namespace BookApplication.Models
{

    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime BirthYear { get; set; }
        public bool IsDeleted { get; set; }
        public List<BookDto> Books { get; set; }
    }

    public class AuthorUpdate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
    }

    public class AuthorCreate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
    }

    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}