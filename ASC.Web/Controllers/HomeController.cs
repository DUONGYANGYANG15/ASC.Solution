using ASC.Solution.Configuration;
using ASC.Solution.Models;
using ASC.Solution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using ASC.Utilities;

namespace ASC.Solution.Controllers
{
    public class HomeController : Controller // Đổi từ AnonymousController thành Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<ApplicationSettings> _settings;

        public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public IActionResult Index()
        {
            // Kiểm tra nếu session có tồn tại trước khi truy cập
            if (HttpContext.Session != null)
            {
                HttpContext.Session.SetSession("Test", _settings.Value);
                var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");
            }

            ViewBag.Title = _settings.Value.ApplicationTitle;
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
