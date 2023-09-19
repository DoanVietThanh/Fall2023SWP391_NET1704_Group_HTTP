namespace DriverLicenseLearningSupport.Payloads.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } 
        public Object Errors { get; set; }
    }
}
