using System;
using System.Collections.Generic;
using Bookshelf.Model;

namespace Bookshelf.Library
{
    public interface ILibraryClient
    {
        BookshelfItem GetBookshelfItem(int bookId);
    }
}