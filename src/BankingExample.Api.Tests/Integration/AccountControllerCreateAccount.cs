using Alba;
using BankingExample.ApiClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingExample.Api.Tests.Integration
{
    public class AccountControllerCreateAccount
    {
        [Test]
        public Task create_account_ok()
        {
            return Application.AlbaHost.Scenario(_ =>
            {
                
                
                _.Post
                    .Json(new { Owner = "TestMe", StartingBalance = 200 })
                    .ToUrl("/api/account/create");
                _.StatusCodeShouldBeOk();
            });

            
            
        }

        [Test]
        public async Task create_account_withclient_ok()
        {
            using (var httpClient = Application.AlbaHost.Server.CreateClient())
            {
                var client = new swagger_banking_exampleClient(httpClient.BaseAddress.ToString(), httpClient);

                var result = await client.CreateAsync(new CreateAccount { Owner = "ClientTest", StartingBalance = 500 });

                Assert.IsNotNull(result);
            }
        }
    }
}
