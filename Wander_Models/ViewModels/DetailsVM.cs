using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander_Models.ViewModels
{
   public class DetailsVM
    {

        public DetailsVM()
        {
            Property = new Property();
        }

        public Property Property { get; set; }

        public bool ExistsInInquiries { get; set; }

    }
}
