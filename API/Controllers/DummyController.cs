using System;
using System.Collections.Generic;
using System.Linq;
using API.Models;
using API.Services;
using API.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/dummyTest")]
        public IActionResult TestAPI()
        {
            return Ok();
        }

    }
}