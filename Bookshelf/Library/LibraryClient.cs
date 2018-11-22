using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Bookshelf.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bookshelf.Library
{
    public class LibraryClient : ILibraryClient
    {
        private readonly IOptions<ServiceDependencies> _serviceDependencies;

        public LibraryClient(IOptions<ServiceDependencies> serviceDependencies)
        {
            _serviceDependencies = serviceDependencies;
        }
        public BookshelfItem GetBookshelfItem(int bookId)
        {
            return GetBooksFromCatalogService(bookId);
        }

        public IEnumerable<BookshelfItem> GetAvailableBooks()
        {
            var bookResource = "/library";
            var response = MakeRequest(bookResource);
            return ConvertToBookRequestItems(response);
        }

        private BookshelfItem GetBooksFromCatalogService(int bookId)
        {
            var bookResource = string.Format(
                "/library/{0}", bookId);
            var response = MakeRequest(bookResource);
            return ConvertToBookRequestItem(response);
        }

        private HttpResponseMessage MakeRequest(string bookResource)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_serviceDependencies.Value.LibraryService);
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(bookResource).Result;
                return response;
            }
        }

        private static BookshelfItem ConvertToBookRequestItem(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<LibraryBook>(result);
            return
                new BookshelfItem(0,
                        books.Id,
                        books.Title
                    );
        }

        private static IEnumerable<BookshelfItem> ConvertToBookRequestItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var books = JsonConvert.DeserializeObject<IEnumerable<LibraryBook>>(result);
            var libraryBooks = books.ToList();
            return libraryBooks.Select(book => new BookshelfItem(0, book.Id, book.Title));
        }

        private class LibraryBook
        {
            public LibraryBook(int Id, string Title, string Summary)
            {
                this.Id = Id;
                this.Title = Title;
                this.Summary = Summary;
            }

            public int Id { get; }
            public string Title { get; }
            public string Summary { get; }
        }
    }
}