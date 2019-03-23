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
            //These functions get called when the provider system needs to be placed 
            //in a Given state.  The name of the state matches the Given in the consumer test
            _verifier.AssignProviderStateCallback("An existing book", _libraryState.AddBooks);
            _verifier.AssignProviderStateCallback("A book does not exist", _libraryState.RemoveBooks);
            //Act
            _verifier.Initialize("http://localhost:5100");
            //Assert
            //Use a Pact Broker instead of manually sharing contracts
            _verifier.VerifyPublishedContract("Library", "Bookshelf", "http://localhost", "latest");
        }
    }
}
