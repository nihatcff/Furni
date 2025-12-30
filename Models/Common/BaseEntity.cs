namespace Furni.Models.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
