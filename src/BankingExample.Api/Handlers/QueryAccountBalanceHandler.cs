using BankingExample.Api.Interfaces;
using BankingExample.Api.Projections;
using Marten;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class QueryAccountBalanceHandler
    {
        public class QueryAccountBalance : IRequest<AccountBalances>
        { 

            [Required]
            public Guid[] Ids { get; set; }


        }

        public class AccountBalances 
        {
            public AccountBalances(IEnumerable<Account> data, long count)
            {
                Data = data;
                Count = count;
            }

            public IEnumerable<Account> Data { get; }
            public long Count { get; }
        }

        public class Handler : IRequestHandler<QueryAccountBalance, AccountBalances>
        {
            private readonly IDocumentStore _store;

            public Handler(IDocumentStore store)
            {
                _store = store;
            }

            public async Task<AccountBalances> Handle(QueryAccountBalance request, CancellationToken cancellationToken)
            {
                IReadOnlyList<Account> accounts;

                //TODO: figure out how to make paging work for this
                using (var session = _store.LightweightSession())
                {
                    accounts = await session.LoadManyAsync<Account>(request.Ids);
                }

                return new AccountBalances(accounts, accounts.Count);
            }
        }
    }
}
