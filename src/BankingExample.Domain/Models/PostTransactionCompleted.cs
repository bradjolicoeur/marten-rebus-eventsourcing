using System;

namespace BankingExample.Domain.Models
{
    public class PostTransactionCompleted
    {
        public Guid Id { get; set; }
        public DateTime Completed { get; set; }
    }
}
