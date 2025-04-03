using Dapper;
using Lab9Startup.Models;
using MySqlConnector;

namespace Lab9Startup.Services
{
    public class BookDbAccessor
    {
        protected MySqlConnection connection;

        public BookDbAccessor()
        {
            // get environemnt variable
            //string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            //string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbHost = "localhost";
            string dbUser = "root";
            string dbPassword = "password";

            var builder = new MySqlConnectionStringBuilder
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "library", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Initialize the database and create the books table
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS books (
                BookId VARCHAR(36) PRIMARY KEY,
                Title VARCHAR(255) NOT NULL,
                Author VARCHAR(255) NOT NULL,
                Description TEXT,
                Category VARCHAR(255)
            )";

            connection.Execute(sql);

            connection.Close();
        }

        /// <summary>
        /// Implement the AddBook method to add a book to the database
        /// </summary>
        /// <param name="book"></param>
        public void AddBook(Book book)
        {
            connection.Open();
            string insertsql = $"Insert into books (BookId, Title, Author, Description, Category) values ('{book.BookId}', '{book.Title}', '{book.Author}', '{book.Description}', '{book.Category}')";
            MySqlCommand insertCommand = new MySqlCommand(insertsql, connection);
            int rowsAffected = insertCommand.ExecuteNonQuery();
            connection.Close() ;
        }

        /// <summary>
        /// Implement the GetBooks method to get all books from the database
        /// </summary>
        /// <returns></returns>
        public List<Book> GetBooks()
        {
            List<Book> booksList = new List<Book>();
            connection.Open() ;
            string sql = $"Select * from books;";
            MySqlCommand sqlCommand = new MySqlCommand(sql, connection);
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                string bookid = reader.GetString(0);
                string title = reader.GetString(1);
                string author = reader.GetString(2);
                string description = reader.GetString(3);
                string category = reader.GetString(4);  
                Book newBook = new Book(bookid,title, author, description, category);
                booksList.Add(newBook);
            }
            connection.Close();
            return booksList;
        }

        /// <summary>
        /// Implement the GetBook method to get a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public Book GetBook(string bookId)
        {
            Book selectedBook = null;
            connection.Open();
            string sql = $"Select * from books WHERE BookId = '{bookId}' ";
            MySqlCommand sqlCommand = new MySqlCommand(sql, connection);
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                string bookid = reader.GetString(0);
                string title = reader.GetString(1);
                string author = reader.GetString(2);
                string description = reader.GetString(3);
                string category = reader.GetString(4);
                selectedBook = new Book(bookid, title, author, description, category);
                
            }
            connection.Close();
            return selectedBook;

        }

        /// <summary>
        /// Implement the UpdateBook method to update a book in the database
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBook(Book book)
        {
            connection.Open();
            string sql = $"UPDATE books set title = '{book.Title}', author = '{book.Author}', description = '{book.Description}', category = '{book.Category}' where bookid = '{book.BookId}'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            int affected = command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Implement the DeleteBook method to delete a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBook(string bookId)
        {
            connection.Open();
            string sql = $"delete from books where bookid = '{bookId}'";
            MySqlCommand command = new MySqlCommand( sql, connection);
            int affected = command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
