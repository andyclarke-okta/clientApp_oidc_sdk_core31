using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Okta.AspNetCore;
using okta_aspnetcore_mvc_example.Models;

namespace okta_aspnetcore_mvc_example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        //[Authorize]
        public ActionResult Login()
        {
            ViewBag.Message = "Your Login Page.";

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var properties = new AuthenticationProperties();
                //without this, the redirect defaults to entry point of initialization
                //properties.RedirectUri = "/Home/PostLogOut";
                return Challenge(properties, OktaDefaults.MvcAuthenticationScheme);

            }

            return RedirectToAction("PostLogin", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            return new SignOutResult(
                new[]
                {
                     OktaDefaults.MvcAuthenticationScheme,
                     CookieAuthenticationDefaults.AuthenticationScheme,
                },
                new AuthenticationProperties { RedirectUri = "/Home/" });


        }

        public ActionResult PostLogin()
        {

            return View();
        }

        public ActionResult PostLogOut()
        {

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View(HttpContext.User.Claims);
        }
    }
}
