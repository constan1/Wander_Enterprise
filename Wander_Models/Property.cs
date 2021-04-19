using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Wander_Models
{
   public class Property
    {


        [Key]

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name="Short Description")]
        public string Description { get; set; }


        [Display(Name = "Long Description")]
        public string Detailed_Description { get; set; }

        public string Type { get; set; }

        [Display(Name="Address")]
        [Required]
        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = " Number of Baths  must be greater than 0")]
        public int Baths { get; set; }

       [Range(1, int.MaxValue, ErrorMessage = " Number of Baths  must be greater than 0")]
        public int Beds { get; set; }

        public int Size { get; set; }

        public double Price { get; set; }


        
        [Display(Name = "Main Image")]
        
        public string Main_Image { get; set; }

       
        [Display(Name = "Secondary Image")]
        
        public string Secondary_Image { get; set; }

       
        [Display(Name = "Third Image")]
        
        public string Third_Image { get; set; }

       
        [Display(Name = "Fourth Image")]
        
        public string Fourth_Image { get; set; }

        
        [Display(Name = "Fifth Image")]
       
        public string Fifth_Image { get; set; }

        
        [Display(Name = "Agent ID")]

        public string Agent_Id { get; set; }




    }
}
