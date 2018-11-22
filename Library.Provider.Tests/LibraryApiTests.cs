using Xunit;
using Xunit.Abstractions;
using ContractTesting;

namespace Library.Provider.Tests
{
    public class LibraryApiTests : IClassFixture<ProviderContractVerifier>
    {
        private readonly ProviderContractVerifier _verifier;
        private readonly LibraryState _libraryState;

        public LibraryApiTests(ProviderContractVerifier verifier, ITestOutputHelper outputHelper)
        {
            _verifier = verifier;
            //Required to get test output in XUnit
            _verifier.TestOutput = outputHelper;
            _libraryState = new LibraryState();
        }

        [Fact]
        public void ShouldExecuteAccordingToContract()
        {
            //Arrange
            _verifier.ProvideState("An existing book", _libraryState.AddBooks);
            _verifier.ProvideState("A book does not exist", _libraryState.RemoveBooks);
            //Act
            _verifier.Initialize("http://localhost:5100");
            //Assert
            _verifier.VerifyPublishedContract("Library", "Bookshelf", "http://localhost", "latest");
        }
    }
}
