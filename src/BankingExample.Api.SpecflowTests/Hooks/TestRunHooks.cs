using Alba;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using ThrowawayDb.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Oakton;

namespace BankingExample.Api.SpecflowTests.Hooks
{
    [Binding]
    public static class TestRunHooks
    {
        // Make this lazy so you don't build it out
        // when you don't need it.
        private static IAlbaHost _host;
        public static IAlbaHost AlbaHost => _host;
        public static ThrowawayDatabase Database;

        [BeforeTestRun]
        public static async Task SetupHost()
        {
            var testHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var rabbitMQ = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "amqp://rabbitmq:rabbitmq@localhost";


            //Create a temporary database for testing
            Database = ThrowawayDatabase.Create(username: "merbes_user", password: "not_magical_scary", host: testHost);

            OaktonEnvironment.AutoStartHost = true;

            _host =  await Alba.AlbaHost.For<global::Program>(builder =>
                {
                    builder.UseSetting("MRBES_DB", Database.ConnectionString);
                    builder.UseSetting("RABBITMQ", rabbitMQ);
                    builder.UseSetting("INPUT_QUEUE", "bankingexampletest.webapp");
                });


        }

        [AfterTestRun]
        public static void TeardownHost()
        {
            if (_host != null)
            {
                _host.Dispose();
            }

            if (Database != null)
            {
                Database.Dispose();
            }
        }
    }
}
