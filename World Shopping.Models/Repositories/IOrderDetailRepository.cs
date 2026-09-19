using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World_Shopping.Models.Models;
using World_Shopping.Web.Entities.Models;

namespace World_Shopping.Models.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    { 
        void Update(OrderDetail orderDetail);
    }
}
