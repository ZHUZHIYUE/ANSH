using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Testing.UI.Models;

namespace Testing.UI.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        public IActionResult Middleware_UseStatusCode (int StatusCode) => new StatusCodeResult (StatusCode);
    }
}