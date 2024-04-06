using BankingExample.Domain.Events;
using Marten;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Wolverine.Http;

namespace BankingExample.Api.Features.ClearDatabase
{
    public static class Endpoint
    {
        [Tags("Admin")]
        [WolverinePost("api/admin/clear")]
        public static async Task<string> Post(IDocumentStore store)
        {
            // Deletes all the documents stored in a Marten database
            await store.Advanced.Clean.DeleteAllDocumentsAsync();


            return "All documents have been deleted";
        }


    }
}
