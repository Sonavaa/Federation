using Microsoft.AspNetCore.Mvc;

namespace FederationTask.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult TeamPage()
        {
            return View();
        }
    }
}
