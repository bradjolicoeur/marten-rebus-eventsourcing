using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class PostTransactionHandler
    {
        public class AccountTransaction : IRequest<TransactionResults>
        {
            public Guid To { get; set; }
            public Guid Account { get; set; }

            public string Description { get; set; }

            public DateTimeOffset Time { get; set; }

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
            public Task<TransactionResults> Handle(AccountTransaction request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new TransactionResults(true, "fake response"));
            }
        }
    }
}
