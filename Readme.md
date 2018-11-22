1. This example requires a [Pact Broker](https://docs.pact.io/getting_started/sharing_pacts) configured on http://localhost

2. The easiest way to set one up is with a [Docker container](https://hub.docker.com/r/dius/pact-broker/).  You also need to configure it with a [Postgresql container](https://github.com/DiUS/pact_broker-docker/blob/master/POSTGRESQL.md).  

3. Use *dotnet run* to startup the Library and Bookshelf services on http://localhost:5100 and http://localhost:5200 respectively 

4. Might be good to check out as close to root drive as possible.  PactNet wraps the Ruby implementation which has some long paths and Windows has directory path length limitations.