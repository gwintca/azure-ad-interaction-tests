using System;
using System.Collections.Generic;
using System.Text;
using AAD_Sample;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Web.InteractionTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("TestAutomation");
            base.ConfigureWebHost(builder);
        }
    }
}
