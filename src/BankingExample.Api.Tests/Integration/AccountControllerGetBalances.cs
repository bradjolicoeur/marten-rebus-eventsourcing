using Alba;
using BankingExample.ApiClient;

namespace BankingExample.Api.Tests.Integration
{
    public class AccountControllerGetBalances
    {
        [Test]
        public Task get_balance_ok()
        {
            return Application.AlbaHost.Scenario(_ =>
            {
                _.Get.Url("/api/account/balances");
                _.StatusCodeShouldBeOk();
            });
        }

        [Test]
        public async Task get_balances_withclient_ok()
        {
            using (var httpClient = Application.AlbaHost.Server.CreateClient())
            {
                var client = new swagger_banking_exampleClient(httpClient.BaseAddress.ToString(), httpClient);

                var result = await client.Balances2Async(null, null, null, null);

                await Verifier.Verify(result);
            }
        }
    }
}
