using BankingExample.Api.Interfaces;
using BankingExample.Api.Projections;
using Marten;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class QueryAccountBalanceHandler
    {
        public class QueryAccountBalance : IRequest<AccountBalances>
        {
            public int Skip { get; internal set; }
            public int Take { get; internal set; }
            public int CurrentPage { get; internal set; }
            public string Search { get; internal set; }
            public Guid[] Ids { get; internal set; }

            public QueryAccountBalance(Guid[] ids, int skip, int take, int currentPage, string search = null)
            {
                Skip = skip;
                Take = take;
                CurrentPage = currentPage;
                Search = search;
                Ids = ids;
            }
        }

        public class AccountBalances : IPagedData<Account>
        {
            public AccountBalances(IEnumerable<Account> data, long count, int page)
            {
                Data = data;
                Count = count;
                Page = page;
            }

            public IEnumerable<Account> Data { get; }
            public long Count { get; }
            public int Page { get; }
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

                return new AccountBalances(accounts, accounts.Count, 1);
            }
        }
    }
}
