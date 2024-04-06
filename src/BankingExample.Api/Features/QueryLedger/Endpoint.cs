using BankingExample.Domain.Projections;
using Marten;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using Wolverine.Http;
using System.Linq;

namespace BankingExample.Api.Features.QueryLedger
{

    public class QueryAccountLedger
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
    public static class Endpoint
    {
        [Tags("Account")]
        [WolverineGet("api/account/ledger")]
        public static async Task<AccountLedger> Post(Guid id, IDocumentSession session)
        {
            var stream = await session.Events.FetchStreamAsync(id, 0);

            return new AccountLedger(stream.Select(o => new EventData
            {
                EventTypeName = o.EventTypeName,
                Data = o.Data,
                Version = o.Version,
                Timestamp = o.Timestamp
            }), stream.Count);
        }


    }
}
