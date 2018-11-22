using ContractTesting;
using Library.Provider.Tests.Middleware;
using Xunit;
using Xunit.Abstractions;

namespace Bookshelf.Provider.Tests
{
    public class BookshelfApiTests : IClassFixture<ProviderContractVerifier>
    {
        private readonly ProviderContractVerifier _verifier;
        private readonly BookshelfState _bookshelfState;

        public BookshelfApiTests(ProviderContractVerifier verifier, ITestOutputHelper outputHelper)
        {
            _verifier = verifier;
            //Required to get test output in XUnit
            _verifier.TestOutput = outputHelper;
            _bookshelfState = new BookshelfState();

            BookshelfMockDependencies.StartMockServices();
        }
        [Fact]
        public void ShouldVerifyContract()
        {
            //Arrange
            _verifier.ProvideState("A User Bookshelf does not exist", _bookshelfState.RemoveAllBookshelves);
            _verifier.ProvideState("A User Bookshelf With 1 Book", _bookshelfState.CreateUserBookshelfOneBook);
            _verifier.ProvideState("A User Bookshelf With 2 Books", _bookshelfState.CreateUserBookshelfTwoBooks);
            //Act
            _verifier.Initialize("http://localhost:5200");
            //Assert
            _verifier.VerifyPublishedContract("Bookshelf", "ApiGateway", "http://localhost", "latest");
        }
    }
}
