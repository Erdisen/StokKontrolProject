using Microsoft.AspNetCore.Mvc;

namespace StokKontrolProject.UI.Areas.UserArea.Controllers
{
    public class HomeController : Controller
    {
        [Area("UserArea")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
