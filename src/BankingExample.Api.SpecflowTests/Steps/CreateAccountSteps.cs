using BankingExample.Api.Client;
using BankingExample.Api.SpecflowTests.Hooks;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BankingExample.Api.SpecflowTests.Steps
{
    [Binding]
    public class CreateAccountSteps
    {
        private Guid _accountId;

        [Given(@"we create an account with starting balance of (.*)")]
        public async Task GivenWeCreateAnAccountWithStartingBalanceOfAsync(double p0)
        {
            using (var httpClient = TestRunHooks.AlbaHost.Server.CreateClient())
            {
                var client = new BankingClient(httpClient.BaseAddress.ToString(), httpClient);

                var result = await client.POST_api_account_createAsync(new CreateAccount { Owner = "ClientTest", StartingBalance = p0 });

                _accountId = result.AccountId;
            }
        }
        
        [Then(@"the account balance should be (.*)")]
        public async Task ThenTheAccountBalanceShouldBe(double p0)
        {
            using (var httpClient = TestRunHooks.AlbaHost.Server.CreateClient())
            {
                var client = new BankingClient(httpClient.BaseAddress.ToString(), httpClient);

                var result = await client.POST_api_account_balancesAsync(new QueryAccountBalances { Ids = new [] { _accountId } });

                result.Data.FirstOrDefault(q => q.Id == _accountId).Balance.Should().Be(p0);
            }
        }
    }
}
