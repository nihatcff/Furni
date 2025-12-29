using System.Threading.Tasks;
using Furni.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Controllers
{
    public class AboutUsController : Controller
    {
        readonly FurniDbContext _context;
        public AboutUsController(FurniDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.AsQueryable().ToListAsync());
        }
    }
}
