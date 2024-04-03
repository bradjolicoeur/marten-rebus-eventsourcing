using BankingExample.Domain.Projections;

namespace BankingExample.Domain.Events
{
    public class AccountCreditSettled : Transaction
    {
        public override void Apply(Account account)
        {

            //Flipping the commented line can simulate aggrigate defect and rebuild
            account.AvailableBalance += Amount;
            //account.Balance += Amount;


            account.Pending -= Amount;
        }

    }
}
