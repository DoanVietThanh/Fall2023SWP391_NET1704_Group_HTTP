using Amazon.Runtime.Internal;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.VnPay.Lib;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Text;
using VNPayPackage.Enums;

namespace DriverLicenseLearningSupport.VnPay.Response
{
    [BindProperties]
    public class VnpayPayResponse
    {
        public SortedList<string, string> responseData
           = new SortedList<string, string>(new VnPayCompare());

        // https://sandbox.vnpayment.vn/tryitnow/Home/VnPayReturn?
        // vnp_Amount=1000000&vnp_BankCode=NCB&vnp_BankTranNo=20170829152730&vnp_CardType=ATM
        // &vnp_OrderInfo=Thanh+toan+don+hang+thoi+gian%3A+2017-08-29+15%3A27%3A02&vnp_PayDate=20170829153052
        // &vnp_ResponseCode=00&vnp_TmnCode=2QXUI4J4&vnp_TransactionNo=12996460&vnp_TxnRef=23597
        // &vnp_SecureHashType=SHA256&vnp_SecureHash=20081f0ee1cc6b524e273b6d4050fefd

        public string? ReturnUrl { get; set; } = string.Empty;
        public decimal? vnp_Amount { get; set; } = 0;
        public string? vnp_BankCode { get; set; } = string.Empty; 
        public string? vnp_BankTranNo { get; set; } = string.Empty;
        public string? vnp_CardType { get; set; } = string.Empty; 
        public string? vnp_OrderInfo { get; set; } = string.Empty;
        public string? vnp_PayDate { get; set; } = string.Empty;
        public string? vnp_ResponseCode { get; set; } = string.Empty;
        public string? vnp_TmnCode { get; set; } = string.Empty;
        public string? vnp_TransactionNo { get; set; } = string.Empty;
        public string? vnp_TransactionStatus { get; set; } = string.Empty;
        public string? vnp_TxnRef { get; set; } = string.Empty;
        public string? vnp_SecureHashType { get; set; } = string.Empty;
        public string? vnp_SecureHash { get; set; } = string.Empty;

        public bool IsValidSignature(string secretKey)
        {
            /// Sorted response data
            MakeResponseData();

            StringBuilder data = new StringBuilder();
            foreach (var keyValuePairs in responseData)
            {
                if (!String.IsNullOrEmpty(keyValuePairs.Value))
                {
                    data.Append(WebUtility.UrlEncode(keyValuePairs.Key)
                        + "=" + WebUtility.UrlEncode(keyValuePairs.Value) + "&");
                }
            }

            var checkSum = HashHelper.HmacSHA512(secretKey,
                data.ToString().Remove(data.Length - 1, 1));
            return checkSum.Equals(this.vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public void MakeResponseData()
        {
            if (vnp_Amount != null)
                responseData.Add("vnp_Amount", vnp_Amount.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_TmnCode))
                responseData.Add("vnp_TmnCode", vnp_TmnCode.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_BankCode))
                responseData.Add("vnp_BankCode", vnp_BankCode.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_BankTranNo))
                responseData.Add("vnp_BankTranNo", vnp_BankTranNo.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_CardType))
                responseData.Add("vnp_CardType", vnp_CardType.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_OrderInfo))
                responseData.Add("vnp_OrderInfo", vnp_OrderInfo.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_TransactionNo))
                responseData.Add("vnp_TransactionNo", vnp_TransactionNo.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_TransactionStatus))
                responseData.Add("vnp_TransactionStatus", vnp_TransactionStatus.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_TxnRef))
                responseData.Add("vnp_TxnRef", vnp_TxnRef.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_PayDate))
                responseData.Add("vnp_PayDate", vnp_PayDate.ToString() ?? string.Empty);
            if (!string.IsNullOrEmpty(vnp_ResponseCode))
                responseData.Add("vnp_ResponseCode", vnp_ResponseCode ?? string.Empty);
        }
    }
}
