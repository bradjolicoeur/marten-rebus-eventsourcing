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
        [ProducesResponseType(StatusCodes.Status201Created, Type= typeof(CreateAccountHandler.CreateAccountResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<CreateAccountHandler.CreateAccountResponse> PostCreate(CreateAccountHandler.CreateAccount command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("debit")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostTransactionHandler.TransactionResults))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<PostTransactionHandler.TransactionResults> PostDebit(PostTransactionHandler.AccountTransaction command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("balances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryAccountBalanceHandler.AccountBalances))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<QueryAccountBalanceHandler.AccountBalances> GetAccountBalances([FromBody] QueryAccountBalanceHandler.QueryAccountBalance query)
        {
            return  await _mediator.Send(query);
        }

        [HttpGet("ledger")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryAccountLedgerHandler.AccountLedger))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<QueryAccountLedgerHandler.AccountLedger> GetAccountLedger([FromQuery] QueryAccountLedgerHandler.QueryAccountLedger query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("balances")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchAccountBalanceHandler.SearchAccountBalances))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResponse))]
        public async Task<SearchAccountBalanceHandler.SearchAccountBalances> SearchAccountBalances([FromQuery] SearchAccountBalanceHandler.QueryAccountBalance query)
        {
            return await _mediator.Send(query);
        }

    }
}
