using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace ContractTesting
{
    public class ProviderContractVerifier : IDisposable
    {
        private IWebHost _webHost;
        private string _providerUri;
        private string _pactServiceUri;
        public ITestOutputHelper TestOutput { get; set; }
        readonly IDictionary<string, Action> _providerStates;

        public ProviderContractVerifier()
        {
            _providerStates = new Dictionary<string, Action>();
        }

        public void Initialize(string providerUri)
        {
            //The actual service needs to be running.  Whether the service is started and hosted
            //within the test or externally will depend on individual needs.  For the purposes of
            //this test, the service will be run initially.
            _providerUri = providerUri;

            _pactServiceUri = "http://localhost:9001";
            //We create a Pact web host which will act as a proxy to the actual provider that is being 
            //verified.  This is required so we can inject the LibraryStateMiddleware into the OWIN pipeline
            //which will handle the /provider-states endpoint calls. /provider-states endpoint calls are used to
            //setup data for your tests.
            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(new ProviderStates(_providerStates));
                })
                .UseStartup<ProviderTestStartup>()
                .Build();

            _webHost.Start();
        }

        public void ProvideState(string name, Action setup)
        {
            _providerStates.Add(name,setup);
        }

        public void VerifyContract(string serviceProvider, string serviceConsumer, string pathToContract)
        {
            var config = CreatePactVerifierConfiguration();

            //Act /Assert
            IPactVerifier pactVerifier = new PactVerifier(config);

            //Configure the endpoint which will be invoked to arrange the 
            //test scenarios.
            pactVerifier.ProviderState($"{_pactServiceUri}/provider-states")
                //The actual service that is being tested
                .ServiceProvider(serviceProvider, _providerUri)
                //The test is making sure our service honors the contract with
                //this consumer.
                .HonoursPactWith(serviceConsumer)
                //The contract being used for the test.
                //Manually sharing contracts is not recommended practice
                //The best approach is to publish a pact contract to a "broker" after
                //the consumer tests are successfully executed.
                .PactUri(pathToContract)
                //Verify call similar to what we've seen with Mocking in unit test calls
                .Verify();
        }

        public void VerifyPublishedContract(string serviceProvider, string serviceConsumer, string brokerUrl, string contractVersion)
        {
            var pathToContract =
                $"{brokerUrl}/pacts/provider/{serviceProvider}/consumer/{serviceConsumer}/{contractVersion}";
            VerifyContract(serviceProvider, serviceConsumer, pathToContract);
        }

        private PactVerifierConfig CreatePactVerifierConfiguration()
        {
            //Instead of hardcoding, this could be an Environment Variable("BUILD_NUMBER");
            var buildNumber = "1.0.0";

            // Arrange
            var config = new PactVerifierConfig
            {
                // We default to using a ConsoleOutput,
                // however xUnit 2 does not capture the console output,
                // so a custom outputter is required.
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(TestOutput)
                },

                // Output verbose verification logs to the test output
                Verbose = true,
                // Required settings for Pact to work properly
                ProviderVersion = !string.IsNullOrEmpty(buildNumber) ? buildNumber : null,
                PublishVerificationResults = !string.IsNullOrEmpty(buildNumber)
            };
            return config;
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _webHost.StopAsync().GetAwaiter().GetResult();
                _webHost.Dispose();
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}