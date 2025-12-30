using System.ComponentModel.DataAnnotations;

namespace Furni.Models
{
    public class Service:BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(64)]
        public string Description { get; set; }
        [Required]
        public string ImageName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
