using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Shared.Shared
{
    public class BaseentitySoftDelete : BaseEntity
    {
        public bool Deleted { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDateTime { get; set; }
        public Guid? DeletedBy { get; set; }

    }
}
