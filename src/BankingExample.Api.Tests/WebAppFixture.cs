using Alba;
using Oakton;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Wolverine;

namespace BankingExample.Api.Tests
{
    public class WebAppFixture : IAsyncLifetime
    {
        public IAlbaHost AlbaHost = null!;
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder().Build();
        public async Task InitializeAsync()
        {

            //Create a temporary database for testing
            await _postgreSqlContainer.StartAsync();
            await _rabbitMqContainer.StartAsync();

            OaktonEnvironment.AutoStartHost = true;

            AlbaHost = await Alba.AlbaHost.For<global::Program>(builder =>
            {
                // Configure all the things

                builder.ConfigureServices(services =>
                {
                    services.DisableAllExternalWolverineTransports();
                });

                builder.UseSetting("MRBES_DB", _postgreSqlContainer.GetConnectionString());
                builder.UseSetting("RABBITMQ", _rabbitMqContainer.GetConnectionString());
                builder.UseSetting("INPUT_QUEUE", "bankingexampletest.webapp");
            });
        }

        public async Task DisposeAsync()
        {
            await AlbaHost.DisposeAsync();

            await _postgreSqlContainer.DisposeAsync();
            await _rabbitMqContainer.DisposeAsync();
        }
    }
}
