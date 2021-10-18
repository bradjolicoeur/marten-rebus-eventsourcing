using BankingExample.Api.Contracts.Commands;
using BankingExample.Api.Events;
using BankingExample.Api.Projections;
using Marten;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Api.BusHandlers
{
    public class PostTransactionHandler : IHandleMessages<PostTransaction>
    {
        private readonly IDocumentStore _store;

        public PostTransactionHandler(IDocumentStore store)
        {
            _store = store;
        }
        public Task Handle(PostTransaction message)
        {
            using (var session = _store.OpenSession())
            {
                var account = session.Load<Account>(message.From);

                var spend = new AccountDebitSettled
                {
                    Amount = message.Amount,
                    From = message.From,
                    To = message.To,
                    Description = message.Description,
                };

                session.Events.Append(spend.From, spend);
                session.Events.Append(spend.To, spend.ToCreditSettled());

                // commit these changes
                session.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
