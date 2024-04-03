using BankingExample.Contracts.Response;
using BankingExample.Domain.Projections;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDocumentStore _store;

        public AdminController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet("clear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public ActionResult<string> ClearDatabase()
        {
            
            // Deletes all the documents stored in a Marten database
            _store.Advanced.Clean.DeleteAllDocuments();

            return Ok("All Documents Deleted");

        }

        [HttpGet("rebuild")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<string>> RebuildAccount()
        {

            throw new NotImplementedException();

            //// Deletes all the documents stored in a Marten database
            //using var daemon = _store.BuildProjectionDaemon();

            //await daemon.RebuildProjection<Account>(new CancellationToken());

            //return Ok("Rebuild Account Completed");

        }
    }
}
