
namespace Furni.ViewModels.ServiceViewModel;

public class UpdateServiceVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> EmployeeIds { get; set; }
    public string? ImagePath { get; set; }
    public IFormFile Image { get; set; }
    public string? ImageUrl { get; set; }
}