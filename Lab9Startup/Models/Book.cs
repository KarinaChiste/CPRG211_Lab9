namespace Lab9Startup.Models
{
    public class Book
    {
        public string BookId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public Book(string bookId, string title, string author, string description, string category)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            Description = description;
            Category = category;
        }

        public Book() { }
    }
}
