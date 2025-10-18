using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infrastructure.Dto.UserManagement
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }       
        [Required]
        public string PersonalId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Gender { get; set; }
        public bool? Active { get; set; }
        public bool? Married { get; set; }
        [Required]
        public string NationalCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string StampNumber { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? LastLoginDate { get; set; }    
    }
}