namespace Brunchie.Models
{
    public class CustomerSupportTicket
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsResolved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateResolved { get; set; }
    }
}
