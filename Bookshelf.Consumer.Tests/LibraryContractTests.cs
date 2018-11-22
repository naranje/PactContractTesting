using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Bookshelf.Library;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace Bookshelf.Consumer.Tests
{
    public class LibraryContractTests : IClassFixture<LibraryContractServer>
    {
        private readonly IMockProviderService _mockLibraryService;
        private readonly string _mockLibraryServiceBaseUri;

        public LibraryContractTests(LibraryContractServer contractServer)
        {
            _mockLibraryService = contractServer.MockProviderService;
            _mockLibraryServiceBaseUri = contractServer.MockProviderServiceUri;
        }

        [Fact]
        public void ShouldGetBooks()
        {
            _mockLibraryService
                //This is essentially the name of this test.  If this test fails on the provider side, this is how 
                //the provider will be able to tell what the consumer is trying to do.
                .UponReceiving("A GET request for a list of books")
                //The request that will be sent from the consumer to the provider
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/library"
                })
                //The expected provider reply. We use this statement to setup our reply for the mock library service
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new []
                    {
                        new
                        {
                            id = 1,
                            title = "Code Complete (Microsoft Programming)"
                        },
                        new
                        {
                            id = 2,
                            title = "The IT Consultant : A Commonsense Framework for Managing the Client Relationship"
                        },
                        new
                        {
                            id = 3,
                            title = "Object Oriented Perl: A Comprehensive Guide to Concepts and Programming Techniques"
                        }
                    }
                });

            var serviceDependenciesMock = new Mock<IOptions<ServiceDependencies>>();
            serviceDependenciesMock.Setup(options => options.Value)
                .Returns(new ServiceDependencies() { LibraryService = _mockLibraryServiceBaseUri });

            //Act
            var result = new LibraryClient(serviceDependenciesMock.Object).GetAvailableBooks().ToList();

            // Assert
            //Although we check for specific values here, to make these tests less fragile the recommended approach would be to
            //check for the structure of the responses rather than the exact values.  For example, we can assert that the
            //Id is a valid guid and that the Title is not blank.
            result.Should().HaveCount(3);
            result[0].BookLibraryId.Should().Be(1);
            result[0].Title.Should().Be("Code Complete (Microsoft Programming)");
        }

        [Fact]
        public void ShouldGetBookInfo()
        {
            //Arrange
            _mockLibraryService
                //the Given method is used to tell the provider (in this case the Library service) what condition 
                //it needs to be in, in order for this test to successfully pass when it is verified on the provider
                .Given("An existing book")
                .UponReceiving("A GET request for book information")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/library/9"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        id = 9,
                        title = "Estimating Software Costs (Software Development Series)"
                    }
                });

            var serviceDependenciesMock = new Mock<IOptions<ServiceDependencies>>();
            serviceDependenciesMock.Setup(options => options.Value)
                .Returns(new ServiceDependencies() {LibraryService = _mockLibraryServiceBaseUri });

            //Act
            var result = new LibraryClient(serviceDependenciesMock.Object).GetBookshelfItem(9);

            // Assert
            result.BookLibraryId.Should().Be(9);
            result.Title.Should().Be("Estimating Software Costs (Software Development Series)");
        }

        [Fact]
        public void ShouldNotGetBookInfo()
        {
            //Arrange
            _mockLibraryService
                .Given("A book does not exist")
                .UponReceiving("A GET request for book information with an invalid book id")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/library/9"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var serviceDependenciesMock = new Mock<IOptions<ServiceDependencies>>();
            serviceDependenciesMock.Setup(options => options.Value)
                .Returns(new ServiceDependencies() { LibraryService = _mockLibraryServiceBaseUri });

            //Act
            Action a = () => new LibraryClient(serviceDependenciesMock.Object).GetBookshelfItem(9);

            // Assert
            a.Should().Throw<HttpRequestException>();
        }
    }
}
