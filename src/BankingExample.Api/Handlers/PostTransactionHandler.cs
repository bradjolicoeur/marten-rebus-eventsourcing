using BankingExample.Api.Events;
using BankingExample.Api.Projections;
using Marten;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class PostTransactionHandler
    {
        public class AccountTransaction : IRequest<TransactionResults>
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

        public class Handler : IRequestHandler<AccountTransaction, TransactionResults>
        {
            private readonly IDocumentStore _store;

            public Handler(IDocumentStore store)
            {
                _store = store;
            }

            public Task<TransactionResults> Handle(AccountTransaction request, CancellationToken cancellationToken)
            {
                TransactionResults result;

                using (var session = _store.OpenSession())
                {
                    var account = session.Load<Account>(request.From);

                    var spend = new AccountDebited
                    {
                        Amount = request.Amount,
                        From = request.From,
                        To = request.To,
                        Description = request.Description,
                    };

                    if (account.HasSufficientFunds(spend))
                    {
                        // should not get here
                        session.Events.Append(spend.From, spend);
                        session.Events.Append(spend.To, spend.ToCredit());
                        result = new TransactionResults(true, request.Description);
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

                }
                
                return Task.FromResult(result);
            }
        }
    }
}
