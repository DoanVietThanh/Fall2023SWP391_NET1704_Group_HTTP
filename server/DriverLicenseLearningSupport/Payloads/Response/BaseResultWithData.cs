namespace DriverLicenseLearningSupport.Payloads.Response
{
    public class BaseResultWithData<T> : BaseResult
    {
        public T? Data { get; set; }

        public void Set(bool success, string message, T data)
        {
            Data = data;
            Success = success;
            Message = message;
        }
    }
}
