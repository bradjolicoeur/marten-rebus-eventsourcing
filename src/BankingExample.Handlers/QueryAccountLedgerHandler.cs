using BankingExample.Api.Contracts.Interfaces;
using Marten;
using Marten.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankingExample.Handlers
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
            public AccountLedger(IEnumerable<EventData> data, long count)
            {
                Data = data;
                Count = count;
            }

            public IEnumerable<EventData> Data { get; }
            public long Count { get; }

        }

        public class EventData
        {
            public string EventTypeName { get; set; }
            public object Data { get; set; }
            public long Version { get; set; }
            public DateTimeOffset Timestamp { get; set; }
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

                    return new AccountLedger(stream.Select(o => new EventData 
                        { EventTypeName = o.EventTypeName, 
                            Data = o.Data, 
                            Version = o.Version, 
                            Timestamp = o.Timestamp
                        }), stream.Count);
                }
            }
        }
    }
}
