using System;
using ContractTesting;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace ApiGateway.Consumer.Tests
{
    public class BookshelfContractServer : ContractServer
    {
        public BookshelfContractServer() : base("ApiGateway", "Bookshelf", @"..\..\..\..\pacts")
        {
        }
    }
}