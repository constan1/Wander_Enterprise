using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        internal DbSet<Address> dbSet;

        public PropertyRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
            this.dbSet = _db.Set<Address>();

        }

        public IEnumerable<SelectListItem> GetAllDropDown(string obj, Expression<Func<Address, bool>> filter = null, Func<IQueryable<Address>, IOrderedQueryable<Address>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            List<SelectListItem> slc = new List<SelectListItem>();
            IQueryable<Address> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
                query.ToList();

                for(var i=0; i<query.ToList().Count; i++)
                {
                    slc.Add(new SelectListItem
                    {
                        Text = query.ToList()[i].Street + ",," + query.ToList()[i].City + "," + query.ToList()[i].Province,
                        Value = query.ToList()[i].Id.ToString()
                    });
                }
                
            }
          
         
            return slc;
           
        }

        public void Update(Property obj)
        {
            _db.Property.Update(obj);
        }
    }
}
