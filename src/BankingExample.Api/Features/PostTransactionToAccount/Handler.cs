using BankingExample.Contracts.Commands;
using BankingExample.Domain.Events;
using BankingExample.Domain.Models;
using BankingExample.Domain.Projections;
using Marten;
using System;
using System.Threading.Tasks;

namespace BankingExample.Api.Features.PostTransactionToAccount
{
    public static class Handler
    {

        public static async Task Handle(PostTransaction message, IDocumentSession session)
        {
            //var messageId = Guid.Parse(_messageContext.Headers[Headers.MessageId]);

            ////Make sure we have not already processed this message
            //var processed = await session.LoadAsync<PostTransactionCompleted>(messageId);
            //if (processed != null)
            //{
            //    //We have already processed this message
            //    return;
            //}

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
            //session.Store(new PostTransactionCompleted { Id = messageId, Completed = DateTime.UtcNow });

            // commit these changes
            session.SaveChanges();
        }
    }
}
