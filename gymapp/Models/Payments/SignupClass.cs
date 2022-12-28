using App.Models.Classes;

namespace App.Models.Payments
{
    public class SignupClass
    {
        public int PaymentId { get; set; }

        public Payment Payment { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }

        public DateTime SignupDate { get; set; }
    }
}