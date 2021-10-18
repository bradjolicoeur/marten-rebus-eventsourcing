using BankingExample.Api.Contracts.Response;
using BankingExample.Api.Handlers;
using BankingExample.Api.Projections;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(CreateAccountHandler.CreateAccountResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<CreateAccountHandler.CreateAccountResponse>> PostCreate(CreateAccountHandler.CreateAccount command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("debit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostTransactionHandler.TransactionResults))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<PostTransactionHandler.TransactionResults>> PostDebit(PostTransactionHandler.AccountTransaction command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("balances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryAccountBalanceHandler.AccountBalances))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<QueryAccountBalanceHandler.AccountBalances>> GetAccountBalances([FromBody] QueryAccountBalanceHandler.QueryAccountBalance query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("ledger")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryAccountLedgerHandler.AccountLedger))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<QueryAccountLedgerHandler.AccountLedger>> GetAccountLedger([FromQuery] QueryAccountLedgerHandler.QueryAccountLedger query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("balances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchAccountBalanceHandler.SearchAccountBalances))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<ActionResult<SearchAccountBalanceHandler.SearchAccountBalances>> SearchAccountBalances([FromQuery] SearchAccountBalanceHandler.QueryAccountBalance query)
        {
            var result =  await _mediator.Send(query);
            return Ok(result);
        }

    }
}
