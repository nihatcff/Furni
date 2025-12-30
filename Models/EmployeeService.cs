namespace Furni.Models
{
    public class EmployeeService:BaseEntity
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public Service Service { get; set; }
        public int ServiceId { get; set; }  
    }
}
