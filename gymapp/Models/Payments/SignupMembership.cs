using System.ComponentModel.DataAnnotations.Schema;
using App.Models.Memberships;

namespace App.Models.Payments
{
    [Table("SignupMemberships")]
    public class SignupMembership
    {
        public int PaymentId { get; set; }

        public Payment Payment { get; set; }

        public int MembershipId { get; set; }

        public Membership Membership { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }

        public DateTime SignupDate { get; set; }
    }
}