namespace Presentation_Layer.ErrorResponse
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Details { get; set; }

        // Constructor
        public ErrorResponse(string message, string details = null)
        {
            Message = message;
            Details = details;
        }
    }
}
