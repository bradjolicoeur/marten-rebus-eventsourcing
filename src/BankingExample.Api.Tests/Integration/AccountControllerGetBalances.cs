using Alba;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
