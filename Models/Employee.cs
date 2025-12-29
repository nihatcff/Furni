using System.ComponentModel.DataAnnotations;


namespace Furni.Models
{
    public class Employee: BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }
        [Required]
        public string Position { get; set; }
        public string? Desciption { get; set; }
        public string ImageName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
