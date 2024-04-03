using BankingExample.Domain.Projections;

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
