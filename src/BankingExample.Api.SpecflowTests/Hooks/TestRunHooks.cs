using Alba;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using ThrowawayDb.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BankingExample.Api.SpecflowTests.Hooks
{
    [Binding]
    public static class TestRunHooks
    {
        // Make this lazy so you don't build it out
        // when you don't need it.
        private static Lazy<IAlbaHost> _host;
        public static IAlbaHost AlbaHost => _host.Value;
        public static ThrowawayDatabase Database;

        [BeforeTestRun]
        public static void SetupHost()
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

        [AfterTestRun]
        public static void TeardownHost()
        {
            if (_host.IsValueCreated)
            {
                _host.Value.Dispose();
            }

            if (Database != null)
            {
                Database.Dispose();
            }
        }
    }
}
