using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander_Models.ViewModels
{
  public class OrderVM
    {
       public OrderDetails orderDetails { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }

    }
}
