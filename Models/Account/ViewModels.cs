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
        
        [Display(Name="Last Name")]
        [Required]
        [MinLength(2)]
        public string last_name {get;set;}
        
        [Display(Name="Email")]
        [Required]
        [MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string email {get;set;}
        
        [Display(Name="Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public int phone { get; set; }
        
        [Display(Name="Password")]
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        
        [Display(Name="Confirm password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string pw_confirm { get; set; }
    }
    public class LogUser
    {
        [Display(Name="Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name="Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class NewAddress
    {
        [Display(Name="Address1")]
        [Required]
        [MinLength(6)]
        public string address_line_one { get; set; }
        
        [Display(Name="Address2")]
        [MinLength(4)]
        public string address_line_two { get; set; }

        [Display(Name="City/Territory")]
        [Required]
        [MinLength(2)]
        public string city { get; set; }

        
        [Display(Name="State/Province")]
        public string state_or_province { get; set; }

        [Display(Name="Zip/Postal")]
        [Required]
        public int zip_or_postal { get; set; }

        [Display(Name="Country")]
        public string country { get; set; }
    }

}