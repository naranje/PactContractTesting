using ContractTesting;

namespace Bookshelf.Consumer.Tests
{
    public class LibraryContractServer : ContractServer
    {
        public LibraryContractServer() : base(consumer: "Bookshelf", provider: "Library", contractDirectory: @"..\..\..\..\pacts")
        {
        }
    }
}