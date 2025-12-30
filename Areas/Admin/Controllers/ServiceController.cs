using Furni.Contexts;
using Furni.Helpers;
using Furni.Models;
using Furni.ViewModels.ServiceViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Furni.Areas.Admin.Controllers;
[Area("Admin")]
public class ServiceController(FurniDbContext _context, IWebHostEnvironment _environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await SendEmployeesWithViewBag();

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceVM VM)
    {
        if (!ModelState.IsValid)
        {
            await SendEmployeesWithViewBag();
            return View(VM);
        }

        foreach (int employeeId in VM.EmployeeIds)
        {

            var isExistEmployee = await _context.Employees.AnyAsync(x => x.Id == employeeId);
            if (!isExistEmployee)
            {
                ModelState.AddModelError("EmployeeIds", "There is no employee with this ID!");
                return View(VM);
            }
        }

        if (!VM.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Size must be maximum 2MB");
            return View(VM);
        }

        if (!VM.Image.CheckType())
        {
            ModelState.AddModelError("Image", "You can only add image types");
            return View(VM);
        }

        string fileName = Guid.NewGuid().ToString() + VM.Image.FileName;
        string path = $"{_environment.WebRootPath}/assets/images";
        string fullPath = Path.Combine(path, fileName);

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            VM.Image.CopyTo(stream);
        }

        Service service = new()
        {
            Name = VM.Name,
            Description = VM.Description,
            ImageName = fileName,
            ImageUrl = fullPath,
            CreatedDate = DateTime.UtcNow.AddHours(4),
            IsDeleted = false
        };



        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null) return NotFound();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        await SendEmployeesWithViewBag();
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (service is null) return NotFound("Service is not found");
        var vm = new UpdateServiceVM()
        {
            Name = service.Name,
            Description = service.Description,
            ImagePath = service.ImageName,
            ImageUrl = service.ImageUrl
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateServiceVM vm)
    {
        if (!ModelState.IsValid)
        {
            await SendEmployeesWithViewBag();
            return View(vm);
        }

        if (!vm.Image?.CheckType() ?? false)
        {
            await SendEmployeesWithViewBag();
            ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil etmelisiniz.");
            return View(vm);
        }

        if (vm.Image?.CheckSize(2) ?? false)
        {
            await SendEmployeesWithViewBag();
            ModelState.AddModelError("Image", "Max size 2mb olmalidir.");
            return View(vm);
        }

        var existingService = await _context.Services.Include(x => x.EmployeeServices).FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (existingService == null)
        {
            await SendEmployeesWithViewBag();
            return NotFound();
        }


        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

        if (vm.Image is { })
        {
            string uniqueImageName = await vm.Image.SaveFileAsync(folderPath);

            string oldImagePath = Path.Combine(folderPath, existingService.ImageName);

            ExtensionMethods.DeleteFile(oldImagePath);

            existingService.ImageName = uniqueImageName;

        }

        existingService.Name = vm.Name;
        existingService.Description = vm.Description;
        existingService.UpdatedDate = DateTime.Now;
        existingService.EmployeeServices.Clear();


        if (vm.EmployeeIds is not null)
        {
            foreach (var empId in vm.EmployeeIds)
            {
                EmployeeService employeeService = new EmployeeService()
                {
                    EmployeeId = empId,
                    ServiceId = existingService.Id
                };


                existingService.EmployeeServices.Add(employeeService);
            }
        }

        _context.Services.Update(existingService);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    private async Task SendEmployeesWithViewBag()
    {
        var employees = await _context.Employees.ToListAsync();
        ViewBag.Employees = employees;
    }
}
