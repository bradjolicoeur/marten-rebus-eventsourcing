using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Api.Models
{
    public class PostTransactionCompleted
    {
        public Guid Id { get; set; }
        public DateTime Completed { get; set; }
    }
}
