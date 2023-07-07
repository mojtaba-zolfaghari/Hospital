
using Hospital.Shared.Repository;
using Inventory.Application.Interfaces.Payment;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Implementation.Payment
{
    public class InvoiceRepository : Repository<Hospital.Domain.PaymentEntity.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(DbContext context) : base(context)
        {
        }
    }
}
