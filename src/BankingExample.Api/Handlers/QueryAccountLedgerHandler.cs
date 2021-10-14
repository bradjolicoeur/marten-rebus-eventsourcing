using BankingExample.Api.Interfaces;
using Marten;
using Marten.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Api.Handlers
{
    public class QueryAccountLedgerHandler
    {
        public class QueryAccountLedger : IRequest<AccountLedger>
        {
      
            [Required]
            public Guid Id { get; set; }

        }

        public class AccountLedger 
        {
            public AccountLedger(IEnumerable<object> data, long count)
            {
                Data = data;
                Count = count;
            }

            public IEnumerable<object> Data { get; }
            public long Count { get; }
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
                    var stream = await session.Events.FetchStreamAsync(request.Id, 0);

                    return new AccountLedger(stream.Select(o => new { o.EventTypeName, o.Data, o.Version, o.Timestamp}), stream.Count);
                }
            }
        }
    }
}
