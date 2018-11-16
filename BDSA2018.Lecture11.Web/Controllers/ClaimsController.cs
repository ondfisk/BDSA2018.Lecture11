using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDSA2018.Lecture11.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        // GET api/claims
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(User.Claims);
        }
    }
}