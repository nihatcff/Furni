namespace Furni.Models
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        
    }
}

