
using BankingExample.Api.Factories;
using BankingExample.Api.Projections;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Postgresql;

namespace BankingExample.Api.Helpers
{
    public static class ConfigureMarten
    {

        public static IServiceCollection AddMartenConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMarten(opts =>
            {
                opts.Connection(configuration["BLASTCMS_DB"]);

                opts.AutoCreateSchemaObjects = AutoCreate.All;

                opts.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline); 

            })
               .BuildSessionsWith<CustomSessionFactory>();


            return services;
        }

       
    }
}
