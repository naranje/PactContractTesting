using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Bookshelf.Provider.Tests
{
    public class BookshelfState
    {
        private readonly IConfigurationRoot _configuration;

        public BookshelfState()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();
        }

        public void RemoveAllBookshelves()
        {
            string DeleteAllBookshelfItems = @"DELETE FROM BookShelfItems";
            string DeleteAllBookshelves = @"DELETE FROM Bookshelf";

            using (var conn = new SqlConnection(_configuration["DatabaseConfiguration:TestDatabaseConnection"]))
            {
                conn.Open();
                conn.Execute(DeleteAllBookshelfItems);
                conn.Execute(DeleteAllBookshelves);
            }
        }

        public void CreateUserBookshelfOneBook()
        {
            RemoveAllBookshelves();

            string AddTestBookshelfForUser = @"INSERT INTO Bookshelf VALUES (1); SELECT CAST(SCOPE_IDENTITY() as int)";
            string AddTestBookshelfItemOneForUser = @"INSERT INTO BookShelfItems (BookShelfId, BookLibraryId, Title) VALUES (@BookshelfId, 1, 'Code Complete (Microsoft Programming)')";

            using (var conn = new SqlConnection(_configuration["DatabaseConfiguration:TestDatabaseConnection"]))
            {
                conn.Open();

                var bookshelfId = conn.Query<int>(
                    AddTestBookshelfForUser).Single();

                conn.Execute(
                    AddTestBookshelfItemOneForUser,
                    new { BookshelfId = bookshelfId });
            }
        }

        public void CreateUserBookshelfTwoBooks()
        {
            RemoveAllBookshelves();

            string AddTestBookshelfForUser = @"INSERT INTO Bookshelf VALUES (1); SELECT CAST(SCOPE_IDENTITY() as int)";
            string AddTestBookshelfItemOneForUser = @"INSERT INTO BookShelfItems (BookShelfId, BookLibraryId, Title) VALUES (@BookshelfId, 2, 'Estimating Software Costs (Software Development Series)')";
            string AddTestBookshelfItemTwoForUser = @"INSERT INTO BookShelfItems (BookShelfId, BookLibraryId, Title) VALUES (@BookshelfId, 1, 'Code Complete (Microsoft Programming)')";

            using (var conn = new SqlConnection(_configuration["DatabaseConfiguration:TestDatabaseConnection"]))
            {
                conn.Open();

                var bookshelfId = conn.Query<int>(
                    AddTestBookshelfForUser).Single();

                conn.Execute(
                    AddTestBookshelfItemOneForUser,
                    new { BookshelfId = bookshelfId });

                conn.Execute(
                    AddTestBookshelfItemTwoForUser,
                    new { BookshelfId = bookshelfId });
            }
        }
    }
}