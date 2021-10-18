using BankingExample.Api.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingExample.Api.Events
{
    public class AccountCreditSettled : Transaction
    {
        public override void Apply(Account account)
        {
            account.AvailableBalance += Amount;
            //account.Balance += Amount;
            account.Pending -= Amount;
        }

    }
}
