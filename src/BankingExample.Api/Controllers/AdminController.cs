using BankingExample.Api.Contracts.Response;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string ClearDatabase()
        {

            // Deletes all the documents stored in a Marten database
            _store.Advanced.Clean.DeleteAllDocuments();

            return "All Documents Deleted";

        }
    }
}
