namespace BeautyMap.NotificationManager.Models
{
    public class Send
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Send(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
