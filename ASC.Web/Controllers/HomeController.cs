using ASC.Solution.Configuration;
using ASC.Solution.Models;
using ASC.Solution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using ASC.Utilities;
using ASC.Web.Controllers;
namespace ASC.Solution.Controllers
{
    public class HomeController : AnonymousController
    {

        private readonly ILogger<HomeController> _logger;
        private IOptions<ApplicationSettings> _settings;
        public HomeController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }
        public IActionResult Index()
        {
            // Set Session
            HttpContext.Session.SetSession("Test", _settings.Value);
            // Get Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");
            // Usage of IOptions
            ViewBag.Title = _settings.Value.ApplicationTitle;

            //Test fail test case
            //ViewData.Model = "Test";
            //throw new Exception("Login Fail!!!");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}