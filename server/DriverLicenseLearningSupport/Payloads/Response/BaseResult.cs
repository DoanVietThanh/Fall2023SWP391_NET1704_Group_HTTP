﻿namespace DriverLicenseLearningSupport.Payloads.Response
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = string.Empty;
        public List<BaseError> Errors { get; set; } = new List<BaseError>();
    }
}
