using Microsoft.AspNetCore.Mvc;

namespace WEB_253551_KORZUN.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["TitleText"] = "Контрольная работа №1";
            return View();
        }
    }
}
