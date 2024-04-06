using System.ComponentModel.DataAnnotations;
using System;
using Wolverine.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingExample.Domain.Events;
using MediatR;
using Marten;
using Microsoft.AspNetCore.Http;

namespace BankingExample.Api.Features.CreateAccount
{
    public class CreateAccount
    {
        public CreateAccount()
        {
            AccountId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        [Required]
        public string Owner { get; set; }
        public Guid AccountId { get; }
        public DateTimeOffset CreatedAt { get; }
        public decimal StartingBalance { get; set; } = 0;
    }

    public class CreateAccountResponse
    {
        public CreateAccountResponse(Guid accountId)
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; }
    }

    public static class Endpoint
    {
        [Tags("Account")]
        [WolverinePost("api/account/create")]
        public static async Task<CreateAccountResponse> Post(CreateAccount command, IDocumentSession session)
        {
            var account = new AccountCreated
            {
                Owner = command.Owner,
                AccountId = command.AccountId,
                StartingBalance = command.StartingBalance,
            };

            session.Events.StartStream(account.AccountId, account);
            await session.SaveChangesAsync();


            return new CreateAccountResponse(command.AccountId);
        }


    }
}
   