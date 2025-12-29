using Furni.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Controllers
{
    public class BlogController : Controller
    {
        readonly FurniDbContext _context;

        public BlogController(FurniDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.Include(x=>x.Employee).AsQueryable().ToListAsync());
        }
    }
}
