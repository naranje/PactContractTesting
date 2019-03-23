using System;
using System.IO;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace ContractTesting
{
    public abstract class ContractServer : IDisposable
    {
        private readonly string _consumer;
        private readonly string _provider;
        private readonly string _contractDirectory;
        private readonly IMockProviderService _mockProviderService;

        public IPactBuilder PactBuilder { get; }

        public IMockProviderService MockProviderService
        {
            get
            {
                _mockProviderService.ClearInteractions();
                return _mockProviderService;
            }
        }
        private int MockServerPort => 9222;
        public string MockProviderServiceUri => String.Format("http://localhost:{0}", MockServerPort);

        private bool _disposedValue; // To detect redundant calls

        protected ContractServer(string consumer, string provider, string contractDirectory)
        {
            _consumer = consumer;
            _provider = provider;
            _contractDirectory = contractDirectory;

            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = contractDirectory,
                LogDir = @".\pact_logs"
            };

            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer(consumer)
                .HasPactWith(provider);

            _mockProviderService = PactBuilder.MockService(MockServerPort);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                // This will save the pact file once finished.
                PactBuilder.Build();
                var pactPublisher = new PactPublisher("http://localhost");

                //You would not be hardcoding the version here, but rather providing it through configuration
                pactPublisher.PublishToBroker(Path.Combine(_contractDirectory, $"{_consumer.ToLower()}-{_provider.ToLower()}.json"), "1.0.0", new[] { "master" });
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}