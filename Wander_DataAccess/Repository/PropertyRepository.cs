using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_DataAccess.Data;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Utilities;

namespace Wander_DataAccess.Repository
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {

        private readonly ApplicationDBContext _db;

        public PropertyRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;

        }

        public IEnumerable<SelectListItem> GetAllDropDown(string obj)
        {
            if (obj == WC.Address)
            {
                return _db.Address.Select(i => new SelectListItem
                {
                    Text = i.Street + "," + i.City + "," + i.Province,
                    Value = i.Id.ToString()
                });
             };
         
            return null;
           
        }

        public void Update(Property obj)
        {
            _db.Property.Update(obj);
        }
    }
}
