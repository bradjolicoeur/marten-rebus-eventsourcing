# Simulated Bank Sample

This is a reference project that is based off of a sample project I found while learning Marten Event Sourcing.  You can find [my rendition of that example here](https://github.com/bradjolicoeur/martin-banking-eventsource).

The simple banking project was a great starting point for getting a basic understanding of the Marten functionality, but it does not provide a good reference on how to use Marten Event Sourcing in a real world application.  This project attempts to simulate what is a slightly more realistic banking scenario in a more holistic pattern.

This project has a set of HTTP Endpoints that accept commands and query the event store.  There is a Swagger page that can be used to try out the different endpoints.  

The Infrastructure required to run or test this project is PostgreSQL and RabbitMQ.  There is a `docker-compose.yml` file adjacent to this readme that I recommend you use to spin up the required infrastructure.  

## Workflow
The general workflow that is implemented is as follows:

1. Create Accounts with a starting balance
2. Post transactions between accounts
3. Pending transactions are sent to a background worker for business rules
4. Transaction is settled

There are endpoints for searching the account balances, listing the ledger (event stream) for each account and getting the balance for a specific set of accounts by id.

## Tech Stack
As a wholistic example, there are some other libraries and background services I introduced to create this example in addition to MartenDb.

- .NET 5
- [Marten](https://martendb.io/) v4
- PostreSQL
- [Rebus](https://github.com/rebus-org/Rebus) (background worker)
- RabbitMQ
- [ThrowawayDB](https://github.com/Zaid-Ajaj/ThrowawayDb) (integration tests)
- [Alba](https://jasperfx.github.io/alba/) (integration test host)

## Asynchronous Background Worker
In this example I was experimenting with Event Sourcing and Event Driven architecture.  It is important to understand that these two architectures are different concepts.  I wanted to understand how I can leverage Event Sourcing for providing an immutable history of each account while offloading the workload to an asynchronous worker.

In this example the use of an asynchronous background worker is gratuitous.  In a real world scenario this worker would probably apply some complex business rules and may call out to an external service to do an [OFAC check](https://home.treasury.gov/policy-issues/financial-sanctions/faqs/topic/1596) before the transaction is settled.  

## Integration Tests
I have also been expanding on how to create better integration tests within build pipelines.  This is where Alba and ThrowawayDB come into play.

Alba is a library that wraps the ASP.NET Test Host and makes it easier to leverage an in-memory host for integration testing.  This means you can run actual http tests against your ASP.NET website or API.  

ThrowawayDb will create a temporary database that can be used for the integration tests.  This makes it easy to create a real test database that is isolated to your build pipeline and is destroyed after the pipeline is completed.  Most of the newer CI/CD solutions have the ability to define containers for hosting infrastructure like databases and message brokers that can be used for integration testing.  