namespace API.DTOs
{
    public class SendCommentDto
    {
        public Guid ActivityId { get; set; }
        public string Body { get; set; }
    }
}
