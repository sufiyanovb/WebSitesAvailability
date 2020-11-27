using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSitesAvailability.Models;
namespace WebSitesAvailability.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly EfGenericRepository<Sites> _sitesRepo;

        public AdminController(ApplicationContext context)
        {
            _sitesRepo = new EfGenericRepository<Sites>(context);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var del = _sitesRepo.FindById(id);
            _sitesRepo.Remove(del);
            return Json(new { success = true, id });
        }

        [HttpGet]
        public IActionResult ListPartial()
        {
            return PartialView(_sitesRepo.Get().OrderByDescending(i => i.Id));
        }
        public IActionResult Index()
        {

            return View(_sitesRepo.Get().OrderByDescending(i => i.Id));
        }

        public ActionResult AddSite()
        {
            return View(new Sites());
        }
    }
}
