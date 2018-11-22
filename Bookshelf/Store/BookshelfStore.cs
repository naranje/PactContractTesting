using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bookshelf.Model;
using Bookshelf.Store;
using Dapper;
using Microsoft.Extensions.Options;

namespace Bookshelf.Store
{
    public class BookshelfStore : IBookshelfStore
    {
        private readonly IOptions<DatabaseConfiguration> _databaseConfiguration;

        public BookshelfStore(IOptions<DatabaseConfiguration> databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        private const string ReadCartSql =
                        @"select Id, UserId  from BookShelf
            where UserId=@UserId";

                    private const string ReadItemsSql =
                        @"select BookShelfItems.BookShelfId, BookShelfItems.BookLibraryId, BookShelfItems.Title from BookShelf, BookShelfItems
            where BookShelfItems.BookShelfId = BookShelf.ID
            and BookShelf.UserId=@UserId";

                    private const string DeleteAllForBookRequestSql =
                        @"delete item from BookShelfItems item
            inner join BookShelf cart on item.BookShelfId = cart.ID
            and cart.UserId=@UserId";

                    private const string AddAllForBookRequestSql =
                        @"insert into BookShelfItems 
            (BookShelfId, BookLibraryId, Title)
            values 
            (@BookShelfId, @BookLibraryId, @Title)";


        public Model.Bookshelf Get(int userId)
        {
            using (var conn = new SqlConnection(_databaseConfiguration.Value.DefaultConnection))
            {
                var bookRequest = conn.QueryFirst<Bookshelf.Model.Bookshelf>(ReadCartSql,
                    new {UserId = userId});

                var currentBookRequestItems = 
                    conn.Query<BookshelfItem>(
                        ReadItemsSql,
                        new {UserId = userId});

                return new Bookshelf.Model.Bookshelf(userId, bookRequest.BookshelfId, currentBookRequestItems);
            }
        }

        public void Save(Model.Bookshelf bookshelf)
        {
            using (var conn = new SqlConnection(_databaseConfiguration.Value.DefaultConnection))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(
                            DeleteAllForBookRequestSql,
                            new {bookshelf.UserId},
                            tx);

                        conn.Execute(
                            AddAllForBookRequestSql,
                            bookshelf.Items,
                            tx);

                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}