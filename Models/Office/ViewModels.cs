using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models
{
    public class EditProduct
    {
        [Required]
        [MinLength(2)]
        public string name {get;set;}
        [Required]
        public float price {get;set;}
        [Required]
        [MinLength(5)]
        public string description {get;set;}
        [Required]
        public int instock_quantity { get; set;}
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