using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_Models;

namespace Wander_DataAccess.Repository.IRepository
{
    public interface IAddressRepository : IRepository<Address>
    {
        void Update(Address obj);
    }
}
