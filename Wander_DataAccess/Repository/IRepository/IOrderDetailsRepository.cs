using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_Models;

namespace Wander_DataAccess.Repository.IRepository
{
   public interface IOrderDetailsRepository: IRepository<OrderDetails>
    {

        void update(OrderDetails obj);
    }
}
