using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Api.Contracts.Commands
{
    public class PostTransaction
    {
        public Guid To { get; set; }
        public Guid From { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Time { get; set; }
        public decimal Amount { get; set; }
    }
}
