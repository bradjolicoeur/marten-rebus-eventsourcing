using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Api.Contracts.Response
{
    public class BadRequestResponse
    {
        public string Message { get; }
        public BadRequestResponse(string message)
        {
            Message = message;
        }
    }
}
