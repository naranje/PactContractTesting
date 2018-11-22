using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace ApiGateway.Consumer.Tests
{
    public class BookshelfContractTests : IClassFixture<BookshelfContractServer>
    {
        private readonly IMockProviderService _mockBookshelfService;
        private readonly string _mockBookshelfServiceBaseUri;

        public BookshelfContractTests(BookshelfContractServer server)
        {
            _mockBookshelfService = server.MockProviderService;
            _mockBookshelfServiceBaseUri = server.MockProviderServiceUri;
        }

        [Fact]
        public void ShouldGetUserBookshelf()
        {
            //Arrange
            _mockBookshelfService
                .Given("A User Bookshelf With 1 Book")
                .UponReceiving("A GET request for a User's bookshelf")
                .With(new ProviderServiceRequest()
                {
                    Method = HttpVerb.Get,
                    Path = "/bookshelf/1"
                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        userId = 1,
                        items = new[]
                        {
                            new
                            {
                                bookLibraryId = 1,
                                title = "Code Complete (Microsoft Programming)"
                            }
                        }
                    }
                });

            //Act
            var result = new BookshelfClient(new Uri(_mockBookshelfServiceBaseUri)).GetUserBookshelf(1);

            // Assert
            result.Items.Should().HaveCount(1);
            result.Items.ToList().Should().Contain(item => item.BookLibraryId == 1);
            result.Items.ToList().Should().Contain(item => item.Title == "Code Complete (Microsoft Programming)");
        }

        [Fact]
        public void ShouldUpdateUserBookshelf()
        {
            //Arrange

            _mockBookshelfService
                .Given("A User Bookshelf With 1 Book")
                .UponReceiving("A POST request for a User's bookshelf")
                .With(new ProviderServiceRequest()
                {
                    Method = HttpVerb.Post,
                    Path = "/bookshelf/1",
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json"}
                    },
                    Body = new[]
                    {
                        new
                        {
                            BookLibraryId = 1,
                            Title = (string) null
                        },
                        new
                        {
                            BookLibraryId = 2,
                            Title = (string) null
                        }
                    }

                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        userId = 1,
                        items = new[]
                        {
                            new
                            {
                                bookLibraryId = 1,
                                title = "Code Complete (Microsoft Programming)"
                            },
                            new
                            {
                                bookLibraryId = 2,
                                title = "Estimating Software Costs (Software Development Series)"
                            }
                        }
                    }
                });

            //Act
            var result = new BookshelfClient(new Uri(_mockBookshelfServiceBaseUri)).UpdateBookshelf(1, new List<BookshelfClient.BookShelfItem>{new BookshelfClient.BookShelfItem(){BookLibraryId = 1}, new BookshelfClient.BookShelfItem() { BookLibraryId = 2 }});

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.ToList().Should().Contain(item => item.BookLibraryId == 1);
            result.Items.ToList().Should().Contain(item => item.Title == "Code Complete (Microsoft Programming)");
        }

        [Fact]
        public void ShouldRemoveUserBookshelf()
        {
            //Arrange
            _mockBookshelfService
                .Given("A User Bookshelf With 2 Books")
                .UponReceiving("A DELETE request for a User's bookshelf")
                .With(new ProviderServiceRequest()
                {
                    Method = HttpVerb.Delete,
                    Path = "/bookshelf/1",
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        userId = 1
                    }
                });

            //Act
            var result = new BookshelfClient(new Uri(_mockBookshelfServiceBaseUri)).RemoveUsersBooks(1);

            // Assert
            result.Items.Should().BeNullOrEmpty();
        }
    }
}