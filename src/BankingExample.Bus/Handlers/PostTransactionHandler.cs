using BankingExample.Contracts.Commands;
using BankingExample.Domain.Events;
using BankingExample.Domain.Models;
using BankingExample.Domain.Projections;
using Marten;
using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Pipeline;
using System;
using System.Threading.Tasks;

namespace BankingExample.Bus.BusHandlers
{
    public class PostTransactionHandler : IHandleMessages<PostTransaction>
    {
        private readonly IDocumentStore _store;
        private readonly IMessageContext _messageContext;

        public PostTransactionHandler(IDocumentStore store, IMessageContext messageContext)
        {
            _store = store;
            _messageContext = messageContext;
        }
        public async Task Handle(PostTransaction message)
        {
            var messageId = Guid.Parse(_messageContext.Headers[Headers.MessageId]);

            using (var session = _store.OpenSession())
            {
                //Make sure we have not already processed this message
                var processed =  await session.LoadAsync<PostTransactionCompleted>(messageId);
                if(processed != null)
                {
                    //We have already processed this message
                    return;
                }

                var account = await session.LoadAsync<Account>(message.From);


                //TODO: implement some business rules here

                var spend = new AccountDebitSettled
                {
                    Amount = message.Amount,
                    From = message.From,
                    To = message.To,
                    Description = message.Description,
                };

                session.Events.Append(spend.From, spend);
                session.Events.Append(spend.To, spend.ToCreditSettled());

                //store document to make this idempotent
                session.Store(new PostTransactionCompleted { Id = messageId, Completed = DateTime.UtcNow });

                // commit these changes
                session.SaveChanges();
            }
        }
    }
}
