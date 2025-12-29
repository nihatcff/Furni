public class UpdateProductVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
    public IFormFile? Image { get; set; }
    public string ImageName { get; set; }
}