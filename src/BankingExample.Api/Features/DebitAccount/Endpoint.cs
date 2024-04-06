using BankingExample.Domain.Events;
using Marten;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using Wolverine.Http;
using AutoMapper;
using BankingExample.Contracts.Commands;
using BankingExample.Domain.Projections;
using Wolverine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingExample.Api.Features.DebitAccount
{
    public class AccountTransaction
    {
        public AccountTransaction()
        {
            Time = DateTime.UtcNow;
        }

        [Required]
        public Guid To { get; set; }

        [Required]
        public Guid From { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public DateTimeOffset Time { get; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal Amount { get; set; }
    }

    public class TransactionResults
    {
        public bool Success { get; }
        public string Message { get; }

        public TransactionResults(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountTransaction, PostTransaction>().ReverseMap();
        }
    }


    public static class Endpoint
    {


        [Tags("Account")]
        [WolverinePost("api/account/debit")]
        public static async Task<TransactionResults> Post(AccountTransaction request, IDocumentSession session, IMapper mapper, IMessageBus bus)
        {
            TransactionResults result;

            var account = session.Load<Account>(request.From);
            if (account == null)
                throw new KeyNotFoundException($"account {request.From} not found");

            PostTransaction command = null;

            var spend = new AccountDebited
            {
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                Description = request.Description,
            };

            if (account.HasSufficientFunds(spend))
            {
                session.Events.Append(spend.From, spend);
                session.Events.Append(spend.To, spend.ToCredit());
                result = new TransactionResults(true, $"{request.Description} - Pending");
                command = mapper.Map<PostTransaction>(request);
            }
            else
            {
                session.Events.Append(account.Id, new InvalidOperationAttempted
                {
                    Description = "Overdraft"
                });
                result = new TransactionResults(false, "Overdraft");
            }

            // commit these changes
            session.SaveChanges();

            if (command != null)
                await bus.SendAsync(command);

            return result;
        }


    }
}
