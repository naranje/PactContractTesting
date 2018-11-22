using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookshelf.Model
{
    public class Bookshelf
    {
        private readonly HashSet<BookshelfItem> _items = new HashSet<BookshelfItem>();

        public long UserId { get; }
        public int BookshelfId { get; }
        public IEnumerable<BookshelfItem> Items => _items;

        public Bookshelf(Int32 Id, Int64 UserId)
        {
            this.UserId = UserId;
            BookshelfId = Id;
        }

        public Bookshelf(int userId, int bookshelfId, IEnumerable<BookshelfItem> items)
        {
            UserId = userId;
            BookshelfId = bookshelfId;
            foreach (var item in items) _items.Add(item);
        }

        public void AddItems(BookshelfItem bookshelfItem)
        {
            bookshelfItem.BookShelfId = BookshelfId;
                _items.Add(bookshelfItem);
        }

        public void RemoveItems(
            int libraryBookId)
        {
            _items.RemoveWhere(i => i.BookLibraryId == libraryBookId);
        }

        public void RemoveAllItems()
        {
            _items.Clear();
        }
    }
}
