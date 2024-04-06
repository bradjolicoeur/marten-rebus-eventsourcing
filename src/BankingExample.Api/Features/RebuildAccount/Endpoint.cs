using Marten;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Wolverine.Http;

namespace BankingExample.Api.Features.RebuildAccount
{
    public static class Endpoint
    {
        [Tags("Admin")]
        [WolverinePost("api/admin/rebuild")]
        public static Task<string> Post(IDocumentStore store)
        {
            throw new NotImplementedException();
        }


    }
}
