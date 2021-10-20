using BankingExample.Domain.Projections;

namespace BankingExample.Domain.Events
{
    public class AccountDebited : Transaction
    {
        public override void Apply(Account account)
        {
            account.AvailableBalance -= Amount;
            //account.Balance -= Amount;
            account.Pending += Amount;
        }


        public AccountCredited ToCredit()
        {
            return new AccountCredited
            {
                Amount = Amount,
                To = From,
                From = To,
                Description = Description
            };
        }

        public override string ToString()
        {
            return $"{Time} Debited {Amount.ToString("C")} to {To}";
        }
    }
}
