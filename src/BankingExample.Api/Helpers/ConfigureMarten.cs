
using BankingExample.Api.Factories;
using BankingExample.Domain.Projections;
using Marten;
using Marten.Events.Daemon.Resiliency;
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

                // Turn on the async daemon in "Solo" mode
                //opts.Projections.AsyncMode = DaemonMode.Solo;

                //Namespace Migration https://martendb.io/events/versioning.html#namespace-migration
                opts.Events.AddEventTypes(new[] 
                { 
                    typeof(BankingExample.Domain.Events.AccountCreated),
                    typeof(BankingExample.Domain.Events.AccountCredited),
                    typeof(BankingExample.Domain.Events.AccountCreditSettled),
                    typeof(BankingExample.Domain.Events.AccountDebited),
                    typeof(BankingExample.Domain.Events.AccountDebitSettled),
                    typeof(BankingExample.Domain.Events.InvalidOperationAttempted),
                });

                opts.Projections.SelfAggregate<Account>(ProjectionLifecycle.Inline); 

            })
               .BuildSessionsWith<CustomSessionFactory>();


            return services;
        }

       
    }
}
