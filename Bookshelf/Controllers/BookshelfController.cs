using System;
using System.Collections.Generic;
using Bookshelf.Library;
using Bookshelf.Model;
using Bookshelf.Store;
using Microsoft.AspNetCore.Mvc;

namespace Bookshelf.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookshelfController : ControllerBase
    {
        private readonly IBookshelfStore _store;
        private readonly ILibraryClient _library;

        public BookshelfController(IBookshelfStore store, ILibraryClient library)
        {
            _store = store;
            _library = library;
        }

        // GET bookshelf/5
        [HttpGet("{userid}")]
        public Model.Bookshelf Get(int userid)
        {
            return _store.Get(userid);
        }

        // POST bookshelf/5
        [HttpPost("{userid}")]
        public Model.Bookshelf Post(int userid, [FromBody] List<BookshelfItem> bookshelfItems)
        {
            var newBookshelf = _store.Get(userid);
            newBookshelf.RemoveAllItems();
            foreach (var item in bookshelfItems)
            {
                newBookshelf.AddItems(_library.GetBookshelfItem(item.BookLibraryId));
            }
            _store.Save(newBookshelf);
            return newBookshelf;
        }

        // DELETE bookshelf/5
        [HttpDelete("{userid}")]
        public Model.Bookshelf Delete(int userid)
        {
            var bookshelf = _store.Get(userid);
            bookshelf.RemoveAllItems();
            _store.Save(bookshelf);
            return bookshelf;
        }
    }
}
