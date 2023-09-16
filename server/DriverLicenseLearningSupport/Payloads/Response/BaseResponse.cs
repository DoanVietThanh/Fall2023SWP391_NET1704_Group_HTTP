namespace DriverLicenseLearningSupport.Payloads.Response
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
