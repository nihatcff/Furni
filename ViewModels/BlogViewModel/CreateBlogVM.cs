using System.ComponentModel.DataAnnotations;

namespace Furni.ViewModels.BlogViewModel
{
    public class CreateBlogVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(5)]
        public string Text { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? PostedDate { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
