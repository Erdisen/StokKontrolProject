using Microsoft.AspNetCore.Mvc;

namespace StokKontrolProject.UI.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
