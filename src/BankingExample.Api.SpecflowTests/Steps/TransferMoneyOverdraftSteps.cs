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
    public class TransferMoneyOverdraftSteps
    {
        private Guid _SamAccountId;
        private Guid _RalphAccountId;
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

        [Given(@"an account for Sam is created with a beginning balance of (.*)")]
        public async Task GivenAnAccountForSamIsCreatedWithABeginningBalanceOf(double p0)
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.CreateAsync(new CreateAccount { Owner = "Sam", StartingBalance = p0 });

            _SamAccountId = result.AccountId;
        }
        
        [Given(@"and account for Ralph is created with a beginning balance of (.*)")]
        public async Task GivenAndAccountForRalphIsCreatedWithABeginningBalanceOf(double p0)
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.CreateAsync(new CreateAccount { Owner = "Ralph", StartingBalance = p0 });

            _RalphAccountId = result.AccountId;
        }
        
        [When(@"(.*) is transfert from Ralph to Sam")]
        public async Task WhenIsTransfertFromRalphToSam(double p0)
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.DebitAsync(new AccountTransaction { Amount = p0, Description = "Test Transfer", From = _RalphAccountId, To = _SamAccountId });

            result.Success.Should().Be(false);
        }
        
        [Then(@"the balance for Sam will be (.*)")]
        public async Task ThenTheBalanceForSamWillBe(double p0)
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.BalancesPOSTAsync(new QueryAccountBalance { Ids = new[] { _SamAccountId } });

            result.Data.FirstOrDefault(q => q.Id == _SamAccountId).Balance.Should().Be(p0);
        }
        
        [Then(@"the balance for Ralph will be (.*)")]
        public async Task ThenTheBalanceForRalphWillBe(double p0)
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);

            var result = await client.BalancesPOSTAsync(new QueryAccountBalance { Ids = new[] { _RalphAccountId } });

            result.Data.FirstOrDefault(q => q.Id == _RalphAccountId).Balance.Should().Be(p0);
        }
        
        [Then(@"Ralphs ledger will include an overdraft event")]
        public async Task ThenRalphsLedgerWillIncludeAnOverdraftEvent()
        {
            var client = new ApiClient(_httpClient.BaseAddress.ToString(), _httpClient);
            var result = await client.LedgerAsync(_RalphAccountId);

            var overdraft = result.Data.FirstOrDefault(q => q.EventTypeName == "invalid_operation_attempted");

            overdraft.Should().NotBeNull();

        }
    }
}
