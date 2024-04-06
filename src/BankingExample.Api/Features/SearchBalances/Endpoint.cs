using BankingExample.Domain.Projections;
using Marten;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Wolverine.Http;
using BankingExample.Api.Contracts.Interfaces;
using Marten.Linq;
using BankingExample.Handlers.Helpers;
using System.Linq;
using Marten.Linq.Parsing.Operators;

namespace BankingExample.Api.Features.SearchBalances
{

    public class SearchAccountBalances : IPagedData<Account>
    {
        public SearchAccountBalances(IEnumerable<Account> data, long count)
        {
            Data = data;
            Count = count;
        }

        public IEnumerable<Account> Data { get; }
        public long Count { get; }
    }

    public static class Endpoint
    {

        [Tags("Account")]
        [WolverineGet("api/account/balances")]
        public static async Task<SearchAccountBalances> Get(IDocumentSession session, int skip = 0, int take = 100, string search = null)
        {
            QueryStatistics stats = null;

            var query = session.Query<Account>()
                .Stats(out stats)

                .If(!string.IsNullOrWhiteSpace(search), x => x.Where(q => q.Owner.Contains(search, StringComparison.OrdinalIgnoreCase)))

                .Skip(skip)
                .Take(take)
                .OrderByDescending(o => o.Owner).AsQueryable();

            var articles = await query.ToListAsync();

            return new SearchAccountBalances(articles, stats.TotalResults);
        }

    }
}
