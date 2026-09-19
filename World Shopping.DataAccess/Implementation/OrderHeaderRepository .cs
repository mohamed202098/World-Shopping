using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Shopping.Models.Models;
using World_Shopping.Models.Repositories;
using World_Shopping.Web.Entities.Models;

namespace World_Shopping.DataAccess.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus)
        {
            var OrderFromDb = _context.OrderHeaders.SingleOrDefault(x => x.Id == id);
            if (OrderFromDb != null)
            {
                OrderFromDb.OrderStatus = OrderStatus;
                OrderFromDb.PaymentDate = DateTime.Now;

                if (PaymentStatus != null)
                {
                    OrderFromDb.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
