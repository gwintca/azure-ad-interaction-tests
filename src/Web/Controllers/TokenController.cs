using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AAD_Sample.Web.Configuration;
using AAD_Sample.Web.Extensions;
using AAD_Sample.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AAD_Sample.Web.Controllers
{
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IOptions<AzureAdOptions> _options;

        public TokenController(IOptions<AzureAdOptions> options)
        {
            _options = options;
        }

        [HttpPost]
        public async Task<IActionResult> GetTokenAsync([FromBody]TokenRequest model)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri("https://login.microsoftonline.com/");

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "grant_type", "password" },
                        { "username", model.Username },
                        { "password", model.Password },
                        { "client_id", _options.Value.ClientId },
                        { "scope", "openid profile User.Read email" },
                        { "client_secret", _options.Value.ClientSecret }
                    });

                var response = await http.PostAsync($"{_options.Value.TenantId}/oauth2/v2.0/token", content);
                var token = await response.ReadAsAsync<dynamic>();

                return Ok(new TokenResponse { Token = token.id_token });
            }
        }
    }
}
