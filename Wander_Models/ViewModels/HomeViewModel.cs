using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander_Utilities;

namespace Wander_Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Property> Properties { get; set; }

     
        public Property prop { get; set; }

   
    }
}
