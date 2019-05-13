using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAD_Sample.Web.Controllers
{
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = new { key = "key1", value = "value1" };
            return Ok(result);
        }
    }
}
