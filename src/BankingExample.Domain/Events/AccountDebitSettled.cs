using BankingExample.Domain.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Domain.Events
{
    public class AccountDebitSettled : Transaction
    {
        public override void Apply(Account account)
        {
            //account.AvailableBalance -= Amount;
            account.Balance -= Amount;
            account.Pending -= Amount;
        }

        public AccountCreditSettled ToCreditSettled()
        {
            return new AccountCreditSettled
            {
                Amount = Amount,
                To = From,
                From = To,
                Description = Description
            };
        }
    }
}
