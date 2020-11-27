using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebSitesAvailability.Models;

namespace WebSitesAvailability.Controllers
{
    public class HomeController : Controller
    {
        private readonly EfGenericRepository<Sites> _sitesRepo;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _sitesRepo = new EfGenericRepository<Sites>(context);
        }

        public IActionResult Index()
        {
      
#if DEBUG
            _sitesRepo.Truncate();
#endif

            var sites = _configuration.GetSection("Init").Get<string[]>();
            foreach (var site in sites)
            {
                _sitesRepo.Create(new Sites() { Url = site });
            }
            ViewBag.RefreshTime = Convert.ToInt32(_configuration["RefreshTime"]);
            return View(_sitesRepo.Get().ToList());
        }
        [HttpGet]
        public IActionResult ListPartial()
        {
            return PartialView(_sitesRepo.Get().ToList());
        }
    }
}
