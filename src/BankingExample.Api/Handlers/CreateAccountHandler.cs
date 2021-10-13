using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace BankingExample.Api.Handlers
{
    public class CreateAccountHandler
    {
        public class CreateAccount : IRequest<CreateAccountResponse>
        {
            public string Owner { get; set; }
            public Guid AccountId { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
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
            public Task<CreateAccountResponse> Handle(CreateAccount request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new CreateAccountResponse(request.AccountId));
            }
        }
    }
}
