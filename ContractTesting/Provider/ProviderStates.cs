using System;
using System.Collections.Generic;

namespace ContractTesting
{
    public class ProviderStates
    {
        public IDictionary<string, Action> ProviderStateActions { get; }

        public ProviderStates()
        {
            ProviderStateActions = new Dictionary<string, Action>();
        }

        public ProviderStates(IDictionary<string, Action> providerStates)
        {
            ProviderStateActions = providerStates;
        }
    }
}