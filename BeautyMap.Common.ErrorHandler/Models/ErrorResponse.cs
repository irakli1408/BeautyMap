namespace BeautyMap.Common.ErrorHandler.Models
{
    public class ErrorResponse
    {
        public string Title { get; set; } = "An error occurred";
        public string? Detail { get; set; }
        public int Status { get; set; }
        public string? Instance { get; set; }
        public string? Type { get; set; }
        public Dictionary<string, object?> Extensions { get; set; } = new();
    }
}
