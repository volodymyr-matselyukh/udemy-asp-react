using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class ProfileActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
    }
}
