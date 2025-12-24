using System.Threading.Tasks;
using Furni.Contexts;
using Furni.Migrations;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]

public class ProductController : Controller
{
    readonly FurniDbContext _context;

    public ProductController(FurniDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if(!ModelState.IsValid) return View(product);
        product.CreatedDate = DateTime.UtcNow.AddHours(4);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if(product == null) return NotFound("Product is not found!");
        //_context.Products.Remove(product);
        product.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound("Product is not found!");
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        var existProduct = await _context.Products.FindAsync(product.Id);
        if (existProduct == null) return NotFound("Product is not found!");
        existProduct.Title = product.Title;
        existProduct.Price = product.Price;
        existProduct.ImageName = product.ImageName;
        existProduct.ImagePath = product.ImagePath;
        existProduct.UpdatedDate = DateTime.UtcNow.AddHours(4);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound("Product is not found!");

        product.IsDeleted = !product.IsDeleted;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }



}
