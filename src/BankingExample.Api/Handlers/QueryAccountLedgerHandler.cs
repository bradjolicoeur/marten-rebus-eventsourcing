using BankingExample.Api.Interfaces;
using Marten;
using Marten.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class QueryAccountLedgerHandler
    {
        public class QueryAccountLedger : IRequest<AccountLedger>
        {
            public int Skip { get; internal set; }
            public int Take { get; internal set; }
            public int CurrentPage { get; internal set; }
            public string Search { get; internal set; }
            public Guid Id { get; internal set; }

            public QueryAccountLedger(Guid id, int skip, int take, int currentPage, string search = null)
            {
                Skip = skip;
                Take = take;
                CurrentPage = currentPage;
                Search = search;
                Id = id;
            }
        }

        public class AccountLedger : IPagedData<object>
        {
            public AccountLedger(IEnumerable<object> data, long count, int page)
            {
                Data = data;
                Count = count;
                Page = page;
            }

            public IEnumerable<object> Data { get; }
            public long Count { get; }
            public int Page { get; }
        }

        public class Handler : IRequestHandler<QueryAccountLedger, AccountLedger>
        {
            private readonly IDocumentStore _store;

            public Handler(IDocumentStore store)
            {
                _store = store;
            }

            public async Task<AccountLedger> Handle(QueryAccountLedger request, CancellationToken cancellationToken)
            {
                using (var session = _store.LightweightSession())
                {
                    var stream = await session.Events.FetchStreamAsync(request.Id);

                    return new AccountLedger(stream.Select(o => new { o.EventTypeName, o.Data, o.Version, o.Timestamp }), stream.Count, request.CurrentPage);
                }
            }
        }
    }
}
