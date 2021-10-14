using Marten;

namespace BankingExample.Api.Factories
{
    public interface ICustomSessionFactory
    {
        IDocumentSession OpenLightWeightSession();
        IDocumentSession OpenSession();
        IQuerySession QuerySession();
    }
}