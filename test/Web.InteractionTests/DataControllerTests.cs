using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Web.InteractionTests.Helpers;
using Xunit;

namespace Web.InteractionTests
{
    public class DataControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public DataControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_returns_Ok()
        {
            using (var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Mock services and repositories here
                });
            }).CreateClient())
            {
                var token = await client.GetTokenAsync("test-user-email", "test-user-password");
                Assert.NotNull(token);

                var response = await client.GetAsync("api/data", message =>
                {
                    message.Headers.Add("Authorization", $"Bearer {token}");
                });

                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task Get_returns_Unauthorized()
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.GetAsync("api/data");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}
