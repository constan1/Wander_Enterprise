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
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public readonly ApplicationDBContext _db;

        public AddressRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;

        }
        public void Update(Address obj)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);

            if(objFromDb != null)
            {
                objFromDb.Street = obj.Street;
                objFromDb.City = obj.City;
                objFromDb.Province = obj.Province;

            }
        }
    }
}
