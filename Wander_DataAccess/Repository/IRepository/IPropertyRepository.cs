using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wander_Models;
using Wander_Utilities;

namespace Wander_DataAccess.Repository.IRepository
{
    public interface IPropertyRepository : IRepository<Property>
    {
        void Update(Property obj);

        IEnumerable<SelectListItem> GetAllDropDown(string obj, Expression<Func<Address, bool>> filter = null, Func<IQueryable<Address>, IOrderedQueryable<Address>> orderBy = null, string includeProperties = null, bool isTracking = true);
        
    }
}
