using Hospital.Shared.Repository;

namespace Inventory.Application.Interfaces.Payment
{
    public interface IInvoiceRepository : IRepository<Hospital.Domain.PaymentEntity.Invoice>
    {
    }
}
