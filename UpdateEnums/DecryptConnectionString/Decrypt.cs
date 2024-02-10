using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpdateEnums.Extension;

namespace UpdateEnums.DecryptConnectionString
{
    public static class Decrypt
    {
        const string _aesEncryptionPassword = "P@ssw0rd";
        const string _encryptionSalt = "doozlyPass";
        public static string DecryptConnectionStrings(string connectionStrings, string encryptionSalt, string aesEncryptionPassword)
        {
            if (string.IsNullOrEmpty(encryptionSalt))
                encryptionSalt = _encryptionSalt;
            if (string.IsNullOrEmpty(aesEncryptionPassword))
                aesEncryptionPassword = _aesEncryptionPassword;

            var userID = ExtractUserName(connectionStrings).AesDecrypt(aesEncryptionPassword, encryptionSalt);
            var password = ExtractPassword(connectionStrings).AesDecrypt(aesEncryptionPassword, encryptionSalt);

            connectionStrings = Regex.Replace(connectionStrings, @"(?<=User ID(\s)*=)(.*)(?=(;))", userID);
            connectionStrings = Regex.Replace(connectionStrings, @"(?<=Password(\s)*=(\s)*)(.*)", password);

            return connectionStrings;
        }

        public static string DecryptText(string text, string encryptionSalt, string aesEncryptionPassword)
        {
            if (string.IsNullOrEmpty(encryptionSalt))
                encryptionSalt = _encryptionSalt;
            if (string.IsNullOrEmpty(aesEncryptionPassword))
                aesEncryptionPassword = _aesEncryptionPassword;

            var Result = text.AesDecrypt(aesEncryptionPassword, encryptionSalt);



            return Result;
        }
        private static string ExtractUserName(string input)
        {
            try
            {
                var regexMatch = Regex.Match(input, @"(?<=User ID(\s)*=)(.*)(?=(;))");
                return regexMatch.Groups[0].Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string ExtractPassword(string input)
        {
            try
            {
                var regexMatch = Regex.Match(input, @"(?<=Password(\s)*=(\s)*)(.*)");
                return regexMatch.Groups[0].Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
