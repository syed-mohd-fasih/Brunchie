using System.Drawing;

namespace Brunchie.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public enum PaymentMethod
        {

        }
        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed
        }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
