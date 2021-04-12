using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wander_Models.ViewModels
{
    public class PropertyVM
    {

        public Property Property { get; set; }

        public IEnumerable<SelectListItem> AddressList { get; set; }



        
        public List<string> Type_ { get; set; }



    



    }
}
