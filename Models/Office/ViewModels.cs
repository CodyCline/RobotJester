using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models
{
    

    public class ViewProduct //Used for creating and updating a current product
    {
        [Display(Name="Product Name")]
        [Required]
        [MinLength(2)]
        public string name {get;set;}
        
        [Display(Name="Price")]
        [Required]
        public float price {get;set;}
        
        [Display(Name="Description")]
        [Required]
        [MinLength(5)]
        public string description {get;set;}

        [Display(Name="Starting Quantity")]
        [Required]
        public int instock_quantity { get; set;}
        // [Display(Name="Product Weight")]
        
        [Required]
        public float weight { get; set; }
        
        [Required]
        public float x_dimension { get; set; }
        
        [Required]
        public float y_dimension { get; set; }
        
        [Required]
        public float z_dimension { get; set; }
    }
    
}