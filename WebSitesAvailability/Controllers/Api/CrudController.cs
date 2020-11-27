using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebSitesAvailability.Models;
using Microsoft.EntityFrameworkCore;

namespace WebSitesAvailability.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Crud")]
    public class CrudController : Controller
    {
        private readonly ApplicationContext _context;

        public CrudController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSites()
        {
            return Json(new { data = _context.Sites.ToList() });
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> PostSite([FromBody] Sites site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Sites.Add(site);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Данные успешно добавлены!", site });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var site = await _context.Sites.SingleOrDefaultAsync(m => m.Id == id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(site);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Удалено" });
        }

    }
}

