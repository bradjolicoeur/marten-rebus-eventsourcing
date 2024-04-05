
using BankingExample.Domain.Projections;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;


namespace BankingExample.Api.Helpers
{
    public static class ConfigureMarten
    {

        public static MartenServiceCollectionExtensions.MartenConfigurationExpression AddMartenConfig(this IServiceCollection services, IConfiguration configuration)
        {

            return services.AddMarten(opts =>
            {
                var connection = configuration["MRBES_DB"];
                opts.Connection(connection);

                opts.AutoCreateSchemaObjects = AutoCreate.All;

                opts.Projections.Add<AccountProjection>(ProjectionLifecycle.Inline); 

            });
        }

       
    }
}
