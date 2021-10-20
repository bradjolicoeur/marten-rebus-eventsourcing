using BankingExample.Api.Contracts.Interfaces;
using BankingExample.Domain.Projections;
using Marten;
using Marten.Linq;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class SearchAccountBalanceHandler
    {
        public class QueryAccountBalance : IRequest<SearchAccountBalances>
        {

            [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            public int Skip { get; set; } = 0;


            [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            public int Take { get; set; } = 100;

            [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            public int CurrentPage { get; set; } = 1;
            public string Search { get; set; } = null;

        }

        public class SearchAccountBalances : IPagedData<Account>
        {
            public SearchAccountBalances(IEnumerable<Account> data, long count, int page)
            {
                Data = data;
                Count = count;
                Page = page;
            }

            public IEnumerable<Account> Data { get; }
            public long Count { get; }
            public int Page { get; }
        }

        public class Handler : IRequestHandler<QueryAccountBalance, SearchAccountBalances>
        {
            private readonly IDocumentStore _store;

            public Handler(IDocumentStore store)
            {
                _store = store;
            }

            public async Task<SearchAccountBalances> Handle(QueryAccountBalance request, CancellationToken cancellationToken)
            {

                using (var session = _store.LightweightSession())
                {
                    QueryStatistics stats = null;

                    var query = session.Query<Account>()
                        .Stats(out stats)

                        //.If(!string.IsNullOrWhiteSpace(request.Search), x => x.Where(q => q.Owner.Contains(request.Search, StringComparison.OrdinalIgnoreCase)))

                        .Skip(request.Skip)
                        .Take(request.Take)
                        .OrderByDescending(o => o.Owner).AsQueryable();

                    var articles = await query.ToListAsync();

                    return new SearchAccountBalances(articles, stats.TotalResults, request.CurrentPage);
                }


            }
        }
    }
}
