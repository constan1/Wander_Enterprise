using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_Models;
using Wander_Utilities;

namespace Wander_DataAccess.Repository.IRepository
{
    public interface IPropertyRepository : IRepository<Property>
    {
        void Update(Property obj);

        IEnumerable<SelectListItem> GetAllDropDown(string obj);
        
    }
}
