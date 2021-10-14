using BankingExample.Api.Contracts.Response;
using BankingExample.Api.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankingExample.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type= typeof(CreateAccountHandler.CreateAccountResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<CreateAccountHandler.CreateAccountResponse> Create(CreateAccountHandler.CreateAccount command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("debit")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostTransactionHandler.TransactionResults))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<PostTransactionHandler.TransactionResults> Debit(PostTransactionHandler.AccountTransaction command)
        {
            return await _mediator.Send(command);
        }

    }
}
