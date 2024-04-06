using BankingExample.Domain.Events;
using Marten;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using Wolverine.Http;
using BankingExample.Domain.Projections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingExample.Api.Features.QueryAccountBalances
{
    public static class Endpoint
    {
        public class QueryAccountBalances
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

        [Tags("Account")]
        [WolverinePost("api/account/balances")]
        public static async Task<AccountBalances> Post(QueryAccountBalances request, IDocumentSession session)
        {
            IReadOnlyList<Account> accounts;

            //TODO: figure out how to make paging work for this
            accounts = await session.LoadManyAsync<Account>(request.Ids);
           

            return new AccountBalances(accounts, accounts.Count);
        }


    }
}
