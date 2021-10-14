using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BankingExample.Api.Events;
using Marten;
using MediatR;

namespace BankingExample.Api.Handlers
{
    public class CreateAccountHandler
    {
        public class CreateAccount : IRequest<CreateAccountResponse>
        {
            public CreateAccount()
            {
                AccountId = Guid.NewGuid();
                CreatedAt = DateTime.UtcNow;
            }

            [Required]
            public string Owner { get; set; }
            public Guid AccountId { get; }
            public DateTimeOffset CreatedAt { get; }
            public decimal StartingBalance { get; set; } = 0;

        }

        public class CreateAccountResponse
        {
            public CreateAccountResponse(Guid accountId)
            {
                AccountId = accountId;
            }

            public Guid AccountId { get; }
        }

        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {

            }
        }


        public class Handler : IRequestHandler<CreateAccount, CreateAccountResponse>
        {
            private readonly IDocumentStore _store;

            public Handler(IDocumentStore store)
            {
                _store = store;
            }

            public Task<CreateAccountResponse> Handle(CreateAccount request, CancellationToken cancellationToken)
            {
                var account = new AccountCreated
                {
                    Owner = request.Owner,
                    AccountId = request.AccountId,
                    StartingBalance = request.StartingBalance,
                };

                using (var session = _store.OpenSession())
                {
                    session.Events.StartStream(account.AccountId, account);

                    session.SaveChanges();
                }

                return Task.FromResult(new CreateAccountResponse(request.AccountId));
            }
        }
    }
}
