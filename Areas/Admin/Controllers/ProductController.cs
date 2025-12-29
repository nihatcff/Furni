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

    IWebHostEnvironment _environment;
    public ProductController(FurniDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductVM VM)
    {
        if (!ModelState.IsValid) return View(VM);
        
        
        string fileName = Guid.NewGuid().ToString() + VM.Image.FileName;
        string path = $"{_environment.WebRootPath}/assets/images";
        string fullPath = Path.Combine(path, fileName);

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            VM.Image.CopyTo(stream);
        }

        Product product = new Product
        {
            Title = VM.Title,
            Price = VM.Price,
            ImageName = fileName,
            ImagePath = fullPath,
            CreatedDate = DateTime.UtcNow.AddHours(4),
            IsDeleted = false
        };



        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound("Product is not found!");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound("Product is not found!");

        UpdateProductVM vm = new UpdateProductVM()
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price,
            ImageName = product.ImageName
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var existProduct = await _context.Products.FindAsync(vm.Id);
        if (existProduct == null) return NotFound("Product is not found!");
        existProduct.Title = vm.Title;
        existProduct.Price = vm.Price;
        //existProduct.ImagePath = vm.ImagePath;
        existProduct.UpdatedDate = DateTime.UtcNow.AddHours(4);

        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

        if (vm.Image is { })
        {
            string existImagePath = Path.Combine(folderPath, existProduct.ImageName);

            if (System.IO.File.Exists(existImagePath))
            {
                System.IO.File.Delete(existImagePath);
            }

            string uniqueName = Guid.NewGuid().ToString() + vm.Image.FileName;

            string path = Path.Combine(folderPath, uniqueName);

            using FileStream stream = new FileStream(path, FileMode.Create);

            vm.ImageName = uniqueName;

            await vm.Image.CopyToAsync(stream);



        }

        existProduct.ImageName = vm.ImageName;
        _context.Products.Update(existProduct);

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
