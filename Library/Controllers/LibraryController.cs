using System;
using System.Collections.Generic;
using Library.Model;
using Library.Store;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryStore _store;

        public LibraryController(ILibraryStore store)
        {
            _store = store;
        }

        // GET library
        [HttpGet]
        public IEnumerable<LibraryCatalogBook> Get()
        {
            var books = _store.GetListOfBooks(3);
            return books;
        }

        //GET library/5
        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(LibraryCatalogBook))]
        [ProducesResponseType(404)]
        public IActionResult Get(int bookId)
        {
            var book = _store.GetBookById(bookId);
            if (book != null)
                return Ok(book);
            return NotFound();
        }
    }
}
