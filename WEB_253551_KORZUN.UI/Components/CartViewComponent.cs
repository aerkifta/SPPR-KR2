using Microsoft.AspNetCore.Mvc;

namespace WEB_253551_KORZUN.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ViewData["CartTotal"] = "00,0";
            ViewData["CartCount"] = 0;

            return View();
        }
    }
}
