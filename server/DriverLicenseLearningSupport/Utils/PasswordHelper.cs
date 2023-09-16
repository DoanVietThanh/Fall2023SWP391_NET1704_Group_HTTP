using System.Text;

namespace DriverLicenseLearningSupport.Utils
{
    public class PasswordHelper
    {
        public static string Key = "adedfg@@kfxcbv@";

        public static string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
            string encryptedPassword = Convert.ToBase64String(storePassword);
            return encryptedPassword; 
        }

        public static string ConvertToDecrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            byte[] encryptedPassword = Convert.FromBase64String(base64EncodeData);
            string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
            return decryptedPassword;
        }
    }
}
