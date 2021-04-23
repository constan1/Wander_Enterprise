using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_DataAccess.Data;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;

namespace Wander_DataAccess.Repository
{
    public class OrderDetailRepository: Repository<OrderDetails>, IOrderDetailsRepository
    {

        private readonly ApplicationDBContext _db;


        public OrderDetailRepository(ApplicationDBContext db): base(db)
        {
            _db = db;
        }

        public void update(OrderDetails obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
