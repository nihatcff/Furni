using System.ComponentModel.DataAnnotations;

public class CreateProductVM
{
    public string Title { get; set; }
    public double Price { get; set; }
    public IFormFile Image { get; set; }
    
}
