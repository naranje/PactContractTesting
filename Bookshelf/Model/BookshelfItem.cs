using System;

namespace Bookshelf.Model
{
    public class BookshelfItem
    {
        public int BookShelfId { get; set; }
        public int BookLibraryId { get; }
        public string Title { get; }

        public BookshelfItem(int bookShelfId, int bookLibraryId, string Title)
        {
            this.BookShelfId = bookShelfId;
            this.BookLibraryId = bookLibraryId;
            this.Title = Title;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = obj as BookshelfItem;
            return BookLibraryId.Equals(that.BookLibraryId);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return BookLibraryId.GetHashCode();
        }
    }
}