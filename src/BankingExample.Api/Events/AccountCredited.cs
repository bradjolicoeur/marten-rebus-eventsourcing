using BankingExample.Api.Projections;

namespace BankingExample.Api.Events
{
    public class AccountCredited : Transaction
    {
        public override void Apply(Account account)
        {
            //account.AvailableBalance += Amount;
            account.Balance += Amount;
            account.Pending += Amount;
        }


        public AccountDebited ToDebit()
        {
            return new AccountDebited
            {
                Amount = Amount,
                To = From,
                From = To,
                Description = Description
            };
        }

        public override string ToString()
        {
            return $"{Time} Credited {Amount.ToString("C")} From {From}";
        }
    }
}
