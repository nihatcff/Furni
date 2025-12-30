namespace Furni.ViewModels.ServiceViewModel;

public class CreateServiceVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public List<int> EmployeeIds { get; set; }

}
