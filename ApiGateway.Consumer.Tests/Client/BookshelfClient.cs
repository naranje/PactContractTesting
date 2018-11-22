using System;
using System.Collections.Generic;
using RestSharp;

namespace ApiGateway.Consumer.Tests
{
    public class BookshelfClient
    {
        protected internal Uri BookshelfServiceBaseUri;
        public BookshelfClient(Uri bookshelfServiceBaseUri)
        {
            if (bookshelfServiceBaseUri == null)
                BookshelfServiceBaseUri = new Uri("http://localhost:5200");

            BookshelfServiceBaseUri = bookshelfServiceBaseUri;
        }

        public BookShelf GetUserBookshelf(int userId)
        {
            var client = new RestClient {BaseUrl = BookshelfServiceBaseUri};
            var request = new RestRequest {Resource = $"bookshelf/{userId}"};
            var response = client.Execute<BookShelf>(request);
            return response.Data;
        }

        public BookShelf UpdateBookshelf(int userId, List<BookShelfItem> items)
        {
            var client = new RestClient { BaseUrl = BookshelfServiceBaseUri };
            var request = new RestRequest
            {
                Resource = $"bookshelf/{userId}", RequestFormat = DataFormat.Json, Method = Method.POST, 
            };
            request.AddBody(items);
            var response = client.Execute<BookShelf>(request);
            return response.Data;
        }

        public BookShelf RemoveUsersBooks(int userId)
        {
            var client = new RestClient { BaseUrl = BookshelfServiceBaseUri };
            var request = new RestRequest
            {
                Resource = $"bookshelf/{userId}", Method = Method.DELETE
            };
            var response = client.Execute<BookShelf>(request);
            return response.Data;
        }

        public class BookShelf
        {
            public int UserId { get; set; }
            public int BookshelfId { get; set; }
            public IEnumerable<BookShelfItem> Items { get; set; }
        }

        public class Book
        {
            public Book(int bookId)
            {
                BookId = bookId;
            }

            public int BookId { get; set; }
        }

        public class BookShelfItem
        {
            public int BookLibraryId { get; set; }
            public string Title { get; set; }
        }
    }
}