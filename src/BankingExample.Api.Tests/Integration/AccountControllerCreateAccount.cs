using Alba;
using BankingExample.ApiClient;

namespace BankingExample.Api.Tests.Integration
{
    public class AccountControllerCreateAccount : IClassFixture<WebAppFixture>
    {

        public AccountControllerCreateAccount(WebAppFixture app)
        {
            _host = app.AlbaHost;
        }

        private readonly IAlbaHost _host;

        [Fact]
        public Task create_account_ok()
        {
            return _host.Scenario(_ =>
            {

                _.Post
                    .Json(new { Owner = "TestMe", StartingBalance = 200 })
                    .ToUrl("/api/account/create");
                _.StatusCodeShouldBeOk();
            });

        }

        [Fact]
        public async Task create_account_withclient_ok()
        {
            using var httpClient = _host.Server.CreateClient();
            var client = new Client(httpClient.BaseAddress.ToString(), httpClient);

            var result = await client.CreateAsync(new CreateAccount { Owner = "ClientTest", StartingBalance = 500 });

            await Verifier.Verify(result);
        }
    }
}
