using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models
{
    
    public class NewUser
    {
        [Display(Name="First Name")]
        [Required]
        [MinLength(2)]
        public string first_name {get;set;}
        
        [Required]
        [MinLength(2)]
        public string last_name {get;set;}
        
        [Required]
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string email {get;set;}
        
        // [MinLength(10)]
        // public int phone { get; set; }
        
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string pw_confirm { get; set; }
    }
    public class LogUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class NewAddress
    {
        [Required]
        [MinLength(6)]
        public string Address_One { get; set; }
        
        
        public string Address_Two { get; set; }

        [Required]
        [MinLength(2)]
        public string City { get; set; }

        
        public string State_Province { get; set; }

        [Required]
        public int Zip_Postal { get; set; }

        
        public string Country { get; set; }
    }

}