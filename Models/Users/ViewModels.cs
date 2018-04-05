using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotJester.Models
{
    public class UserRole
    {
        public List<User> Users {get;set;}
        public User user {get;set;}
    }
    public class NewUser
    {
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
        [MinLength(10)]
        public int phone { get; set; }
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
        public string LogEmail {get;set;}
        [Required]
        [DataType(DataType.Password)]
        public string LogPassword {get;set;}
    }

}