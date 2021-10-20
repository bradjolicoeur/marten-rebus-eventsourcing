using System;

namespace BankingExample.Contracts.Commands
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
