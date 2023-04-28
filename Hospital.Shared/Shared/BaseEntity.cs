using Hospital.Shared.Utitlities.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Shared.Shared
{
    public class BaseEntity
    {
        [OperatorComparer(Utitlities.PublicEnumes.OperatorComparer.Equals)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [OperatorComparer(Utitlities.PublicEnumes.OperatorComparer.GreaterThanOrEqual)]
        [Column(TypeName = "smalldatetime")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [OperatorComparer(Utitlities.PublicEnumes.OperatorComparer.GreaterThanOrEqual)]
        [Column(TypeName = "smalldatetime")]
        public DateTime? ModifiedDate { get; set; }

        public string Properties { get; set; }
    }
}
