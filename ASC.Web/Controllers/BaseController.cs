using Microsoft.AspNetCore.Authorization;
using ASC.Utilities;
using Microsoft.AspNetCore.Mvc;
namespace ASC.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}