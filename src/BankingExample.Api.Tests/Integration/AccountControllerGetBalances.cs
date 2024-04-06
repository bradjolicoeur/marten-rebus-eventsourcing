using Alba;
using BankingExample.Api.Client;

namespace BankingExample.Api.Tests.Integration
{
    public class AccountControllerGetBalances : IClassFixture<WebAppFixture>
    {
        public AccountControllerGetBalances(WebAppFixture app)
        {
            _host = app.AlbaHost;
        }

        private readonly IAlbaHost _host;

        [Fact]
        public Task get_balance_ok()
        {
            return _host.Scenario(_ =>
            {
                _.Get.Url("/api/account/balances");
                _.StatusCodeShouldBeOk();
            });
        }

        [Fact]
        public async Task get_balances_withclient_ok()
        {
            using var httpClient = _host.Server.CreateClient();
            var client = new BankingClient(httpClient.BaseAddress.ToString(), httpClient);

            var result = await client.GET_api_account_balancesAsync(null, null, null);

            await Verifier.Verify(result);
        }
    }
}
