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
        public string address_line_one { get; set; }
        
        
        public string address_line_two { get; set; }

        [Required]
        [MinLength(2)]
        public string city { get; set; }

        
        public string state_or_province { get; set; }

        [Required]
        public int zip_or_postal { get; set; }

        
        public string country { get; set; }
    }

}