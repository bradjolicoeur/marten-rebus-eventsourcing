using Alba;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ThrowawayDb.Postgres;

namespace BankingExample.Api.Tests
{
    [SetUpFixture]
    public class Application
    {
        // Make this lazy so you don't build it out
        // when you don't need it.
        private static readonly Lazy<IAlbaHost> _host;

        static Application()
        {
            var testHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var rabbitMQ = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "amqp://rabbitmq:rabbitmq@localhost";


            //Create a temporary database for testing
            Database = ThrowawayDatabase.Create(username: "merbes_user", password: "not_magical_scary", host: testHost);

            //Add test configuration into IConfiguration
            var testConfiguration = new Dictionary<string, string>
            {
                {"MRBES_DB", Database.ConnectionString },
                {"RABBITMQ", rabbitMQ  },
                {"INPUT_QUEUE", "bankingexampletest.webapp"},
            };

            _host = new Lazy<IAlbaHost>(() => Program
                .CreateHostBuilder(Array.Empty<string>())
                .ConfigureAppConfiguration(c =>
                {
                    c.AddInMemoryCollection(testConfiguration);
                })
                .StartAlba());
        }

        public static IAlbaHost AlbaHost => _host.Value;
        public static ThrowawayDatabase Database;

        // Make sure that NUnit will shut down the AlbaHost when
        // all the projects are finished
        [OneTimeTearDown]
        public void Teardown()
        {
            if (_host.IsValueCreated)
            {
                _host.Value.Dispose();
            }

            if(Database != null)
            {
                Database.Dispose();
            }
        }
    }
}
