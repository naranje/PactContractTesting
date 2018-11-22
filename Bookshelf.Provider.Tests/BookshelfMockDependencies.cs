using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace Library.Provider.Tests.Middleware
{
    public class BookshelfMockDependencies
    {
        public static void StartMockServices()
        {
            var server = FluentMockServer.Start(new FluentMockServerSettings() { Urls = new[] { "http://localhost:5100/" }, });

            server
                .Given(Request.Create().WithUrl(u => u.Contains("/library/2")).UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(@"{
                            ""id"": ""2"",
                            ""title"": ""Estimating Software Costs (Software Development Series)"",
                            ""summary"": ""Product Description\r\nSoftware development costs get out of control fast, partly because it's so difficult to make accurate estimates in advance. This book gives you tools to bring the process in line with reality. It takes into account metrics, estimation tools, object technology, global cost factors, the effect of multiple platforms, contractual issues, and the threat of litigation."",
                            ""authors"": ""Capers Jones, T. Capers Jones"",
                            ""url"": ""http://www.amazon.ca/Estimating-Software-Costs-Development/"",
                            ""isbn"": ""0079130941"",
                            ""published"": ""1998"",
                            ""publisher"": ""McGraw-Hill Companies"",
                            ""binding"": ""Hardcover""
                        }"));

            server
                .Given(Request.Create().WithUrl(u => u.Contains("/library/1")).UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(@"{
                                ""id"": ""1"",
                                ""title"": ""Code Complete (Microsoft Programming)"",
                                ""summary"": ""Believed by many  to be the best practical  guide to writing commercial software."",
                                ""authors"": ""Steve McConnell"",
                                ""url"": ""http://www.amazon.ca/Complete-Microsoft-Programming-Steve-McConnell/"",
                                ""isbn"": ""1556154844"",
                                ""published"": ""1993"",
                                ""publisher"": ""Microsoft Press"",
                                ""binding"": ""Paperback""
                            }"));
        }
    }
}