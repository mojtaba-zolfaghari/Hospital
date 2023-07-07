using Hospital.Shared.Repository;

namespace Inventory.Application.Interfaces.Payment
{
    public interface IPaymentRepository:IRepository<Hospital.Domain.PaymentEntity.Payment>
    {
    }
}
