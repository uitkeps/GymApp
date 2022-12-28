using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models.Memberships;
using App.Models.Payments;
using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? FullName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? HomeAdress { get; set; }

        // [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public List<Payment>? Payments { get; set; }

        public List<SignupClass>? SignupClasses { get; set; }

        public List<SignupMembership>? SignupMemberships { get; set; }
    }
}