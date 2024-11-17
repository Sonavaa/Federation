using Microsoft.AspNetCore.Mvc;

namespace FederationTask.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult ClubPage()
        {
            return View();
        }
    }
}
