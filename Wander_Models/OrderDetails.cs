using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander_Models
{
    public class OrderDetails
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Property")]
        [Required]
        public int PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Display(Name = "Agent ID")]

        public string Agent_Id { get; set; }


    }
}
