using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Model;

namespace Library.Store
{
    public interface ILibraryStore
    {
        IEnumerable<LibraryCatalogBook> GetListOfBooks(int numberOfProductsToGet);
        LibraryCatalogBook GetBookById(int bookId);
    }
}