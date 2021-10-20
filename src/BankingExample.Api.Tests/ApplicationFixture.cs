using System;
using Alba;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;


namespace BankingExample.Api.Tests
{
    [SetUpFixture]
    public class Application
    {
        // Make this lazy so you don't build it out
        // when you don't need it.
        private static readonly Lazy<IAlbaHost> _host;

        static Application()
        {
            _host = new Lazy<IAlbaHost>(() => Program
                .CreateHostBuilder(Array.Empty<string>())
                //.ConfigureAppConfiguration(c => c.Sources.Add(
                .StartAlba());
        }

        public static IAlbaHost AlbaHost => _host.Value;

        // Make sure that NUnit will shut down the AlbaHost when
        // all the projects are finished
        [OneTimeTearDown]
        public void Teardown()
        {
            if (_host.IsValueCreated)
            {
                _host.Value.Dispose();
            }
        }
    }
}
