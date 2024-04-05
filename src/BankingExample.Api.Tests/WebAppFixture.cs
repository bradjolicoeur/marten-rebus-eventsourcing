using Alba;
using ThrowawayDb.Postgres;
using Wolverine;

namespace BankingExample.Api.Tests
{
    public class WebAppFixture : IAsyncLifetime
    {
        public IAlbaHost AlbaHost = null!;
        public static ThrowawayDatabase Database;
        public async Task InitializeAsync()
        {
            var testHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var rabbitMQ = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "amqp://rabbitmq:rabbitmq@localhost";

            //Create a temporary database for testing
            Database = ThrowawayDatabase.Create(username: "merbes_user", password: "not_magical_scary", host: testHost);

            AlbaHost = await Alba.AlbaHost.For<global::Program>(builder =>
            {
                // Configure all the things

                builder.ConfigureServices(services =>
                {
                    services.DisableAllExternalWolverineTransports();
                });

                builder.UseSetting("MRBES_DB", Database.ConnectionString);
                builder.UseSetting("RABBITMQ", rabbitMQ);
                builder.UseSetting("INPUT_QUEUE", "bankingexampletest.webapp");
            });
        }

        public async Task DisposeAsync()
        {
            await AlbaHost.DisposeAsync();

            Database?.Dispose();
        }
    }
}
