using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Controllers
{
    public class AnonymousController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
