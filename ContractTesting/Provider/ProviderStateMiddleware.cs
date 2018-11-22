using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ContractTesting
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfigurationRoot _configuration;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next, ProviderStates options)
        {
            _next = next;
            _providerStates = options.ProviderStateActions;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private void HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() != HttpMethod.Post.ToString().ToUpper() ||
                context.Request.Body == null) return;

            string jsonRequestBody;

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = reader.ReadToEnd();
            }

            var providerState = JsonConvert.DeserializeObject<ProviderExpectedState>(jsonRequestBody);

            //A null or empty provider state key must be handled
            if (providerState != null && !String.IsNullOrEmpty(providerState.State))
            {
                _providerStates[providerState.State].Invoke();
            }
        }
    }
}
