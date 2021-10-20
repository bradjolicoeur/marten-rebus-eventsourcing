using Alba;
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
    }
}
