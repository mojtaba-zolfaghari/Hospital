using Hospital.Shared.Repository;
using Inventory.Application.Interfaces.Payment;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Implementation.Payment
{
    public class PaymentRepository : Repository<Hospital.Domain.PaymentEntity.Payment>, IPaymentRepository
    {
        public PaymentRepository(DbContext context) : base(context)
        {
        }
    }
}
