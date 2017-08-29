using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Library.Models.LibraryModels;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Library.Models
{
    public interface ILibraryRepository
    {
        void Create(Book book);
        void Create(Author author);
        void DeleteBook(int id);
        void DeleteAuthor(int id);
        Book GetBook(int id);
        Author GetAuthor(int id);
        List<Book> GetBooks();
        List<Author> GetAuthors();
        void Update(Book book);
        void Update(Author author);
    }
    public class LibraryRepository : ILibraryRepository
    {
        string connectionString = null;

        public LibraryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Book book)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (book.Title == null) book.Title = "Untitled";
                var sq = "INSERT INTO Books (Title,Annotation,AuthorRefId) VALUES(@Title,@Annotation,@AuthorRefId)";
                db.Execute(sq, book);
            }
        }

        public void Create(Author author)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (author.Name == null) author.Name = "Unnamed";
                var sq = "INSERT INTO Authors (Name) VALUES(@Name)";
                db.Execute(sq, author);
            }
        }

        public void DeleteAuthor(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Books SET AuthorRefId = NULL WHERE AuthorRefId = @BookId";
                db.Execute(sqlQuery, new { id });
                sqlQuery = "DELETE FROM Authors WHERE AuthorId = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void DeleteBook(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Books WHERE BookId = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public Author GetAuthor(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Author>("SELECT * FROM Authors WHERE AuthorId = @id", new { id }).FirstOrDefault();
            }
        }

        public List<Author> GetAuthors()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Author>("SELECT * FROM Authors").ToList();
            }
        }

        public Book GetBook(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Books AS b \nLEFT JOIN Authors AS a on a.AuthorId = b.AuthorRefId \n  WHERE b.BookId = " + id + "";
                return db.Query<Book, Author, Book>(sql,
                    (bk, ath) =>
                    {
                        bk.Author = ath;
                        return bk;
                    },
            splitOn: "AuthorId").FirstOrDefault();
            }
        }

        public List<Book> GetBooks()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Book, Author, Book>(
                    "SELECT * FROM Books AS b\n" +
                    "LEFT JOIN Authors AS a on a.AuthorId = b.AuthorRefId",
                    (bk, ath) =>
                    {
                        bk.Author = ath;
                        return bk;
                    },
            splitOn: "AuthorId").ToList();
            }
        }

        public void Update(Book book)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Books SET Title = @Title,Annotation = @Annotation, AuthorRefId = @AuthorRefId WHERE BookId = @BookId";
                db.Execute(sqlQuery, book);
            }
        }

        public void Update(Author author)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Books SET Name = @Name WHERE AuthorId = @AuthorId";
                db.Execute(sqlQuery, author);
            }
        }

        public List<Book> GetAuthorPublications(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Book, Author, Book>(
                    "SELECT * FROM Books AS b\n" +
                    "LEFT JOIN Authors AS a on a.AuthorId = b.AuthorRefId\n" +
                    "WHERE b.AuthorRefId == "+id,
                    (bk, ath) =>
                    {
                        bk.Author = ath;
                        return bk;
                    },
            splitOn: "AuthorId").ToList();
            }
        }

        public List<Book> FindBooksByTitle(String name)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Book, Author, Book>(
                    "SELECT * FROM Books AS b\n" +
                    "LEFT JOIN Authors AS a on a.AuthorId = b.AuthorRefId\n" +
                    "WHERE b.Title LIKE '%" + name+"%'",
                    (bk, ath) =>
                    {
                        bk.Author = ath;
                        return bk;
                    },
            splitOn: "AuthorId").ToList();
            }
        }

        public List<Book> FindBooksByAnnotation(String name)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Book, Author, Book>(
                    "SELECT * FROM Books AS b\n" +
                    "LEFT JOIN Authors AS a on a.AuthorId = b.AuthorRefId\n" +
                    "WHERE b.Annotation LIKE '%" + name + "%'",
                    (bk, ath) =>
                    {
                        bk.Author = ath;
                        return bk;
                    },
            splitOn: "AuthorId").ToList();
            }
        }

        public Author GetAuthorByName(string name)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Author>("SELECT * FROM Authors WHERE Name LIKE '%@id%'", new { name }).FirstOrDefault();
            }
        }
    }
}
