using Furni.Contexts;
using Furni.Controllers;
using Furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class EmployeeController : Controller
{
    readonly FurniDbContext _context;

    public EmployeeController(FurniDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Employees.ToListAsync());
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (!ModelState.IsValid) return View(employee);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound("Product is not found!");
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound("Product is not found!");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(Employee employee)
    {
        if (!ModelState.IsValid) return View(employee);
        var existProduct = await _context.Employees.FindAsync(employee.Id);
        if (existProduct == null) return NotFound("Product is not found!");
        existProduct.FirstName = employee.FirstName;
        existProduct.LastName = employee.LastName;
        existProduct.Position = employee.Position;
        existProduct.Desciption = employee.Desciption;
        existProduct.ImageName = employee.ImageName;
        existProduct.ImageUrl = employee.ImageUrl;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
