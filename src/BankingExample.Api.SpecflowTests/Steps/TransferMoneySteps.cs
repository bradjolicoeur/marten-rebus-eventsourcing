using BankingExample.Api.Client;
using BankingExample.Api.SpecflowTests.Hooks;
using FluentAssertions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BankingExample.Api.SpecflowTests.Steps
{
    [Binding]
    public class TransferMoneySteps
    {
        private Guid _BobAccountId;
        private Guid _TimAccountId;
        private HttpClient _httpClient;

        [BeforeScenario]
        public void Setup()
        {
            _httpClient = TestRunHooks.AlbaHost.Server.CreateClient();
        }

        [AfterScenario]
        public void Teardown()
        {
            _httpClient.Dispose();
        }

        [Given(@"an account for Bob is created with a beginning balance of (.*)")]
        public async Task GivenAnAccountForBobIsCreatedWithABeginningBalanceOf(double p0)
        {

            var client = new BankingClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.POST_api_account_createAsync(new CreateAccount { Owner = "Bob", StartingBalance = p0 });

            _BobAccountId = result.AccountId;

        }
        
        [Given(@"and account for Tim is created with a beginning balance of (.*)")]
        public async Task GivenAndAccountForTimIsCreatedWithABeginningBalanceOf(double p0)
        {

            var client = new BankingClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.POST_api_account_createAsync(new CreateAccount { Owner = "Tim", StartingBalance = p0 });

            _TimAccountId = result.AccountId;

        }
        
        [When(@"(.*) is transfert from Bob to Tim")]
        public async Task WhenIsTransfertFromBobToTim(double p0)
        {

            var client = new BankingClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.POST_api_account_debitAsync(new AccountTransaction{ Amount = p0, Description="Test Transfer", From = _BobAccountId, To = _TimAccountId});

            result.Success.Should().Be(true);

        }

        [When(@"we wait (.*) seconds for the transaction to process")]
        public async Task WhenWeWaitSecondsForTheTransactionToProcess(int p0)
        {
            await Task.Delay(1000*p0);
        }


        [Then(@"the balance for Tim will be (.*)")]
        public async Task ThenTheBalanceForTimWillBe(double p0)
        {
            var client = new BankingClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.POST_api_account_balancesAsync(new QueryAccountBalances { Ids = new[] { _TimAccountId } });

            result.Data.FirstOrDefault(q => q.Id == _TimAccountId).Balance.Should().Be(p0);
        }
        
        [Then(@"the balance for Bob will be (.*)")]
        public async Task ThenTheBalanceForBobWillBe(double p0)
        {
            var client = new BankingClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.POST_api_account_balancesAsync(new QueryAccountBalances { Ids = new[] { _BobAccountId } });

            result.Data.FirstOrDefault(q => q.Id == _BobAccountId).Balance.Should().Be(p0);
        }
    }
}
