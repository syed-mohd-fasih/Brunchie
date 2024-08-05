namespace Brunchie.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DateSubmitted { get; set; }
    }
}
