using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Library.Model;
using Microsoft.Extensions.Options;

namespace Library.Store
{
    public class LibraryStore : ILibraryStore
    {
        private readonly IOptions<DatabaseConfiguration> _databaseConfiguration;

        public LibraryStore(IOptions<DatabaseConfiguration> databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public IEnumerable<LibraryCatalogBook> GetListOfBooks(int numberOfProductsToGet)
        {
            string ReadItemsSql =
                $"SELECT TOP {numberOfProductsToGet} Id ,Title ,Summary FROM dbo.Book";

            using (var conn = new SqlConnection(_databaseConfiguration.Value.DefaultConnection))
            {
                return conn.Query<LibraryCatalogBook>(ReadItemsSql);
            }
        }

        public LibraryCatalogBook GetBookById(int bookId)
        {
            string ReadItemsSql =
                $"SELECT Id ,Title ,Summary ,Authors ,Url ,Isbn ,Published ,Publisher ,Binding FROM dbo.Book WHERE Id = @id";

            using (var conn = new SqlConnection(_databaseConfiguration.Value.DefaultConnection))
            {
                var libraryCatalogBooks = conn.Query<LibraryCatalogBook>(ReadItemsSql, new {id = bookId}).ToList();
                return libraryCatalogBooks.Any() ? libraryCatalogBooks.First() : null;
            }
        }
    }
}