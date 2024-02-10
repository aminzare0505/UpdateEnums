using Kama.Library.Helper.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateEnumsInDBFromDll.Extension
{
    public static class Extensions
    {
        public static string GetSecurityStamp(this int validHour)
        {
            return string.Format("{0}|{1}", Guid.NewGuid().ToString("N"), DateTime.Now.AddHours(validHour).Ticks);
        }

        public static string GetDigitsFromString(this string input, int from, int to)
        {
            return new string(input.Where(char.IsDigit).ToArray()).Substring(from, to);
        }

        public static string HashMd5(this string plainText)
        {
            return Kama.Library.Helper.Security.HashMd5.Hash(plainText);
        }

        public static string HashText(this string plainText)
        {
            return Kama.Library.Helper.Security.HashSHA256.Hash("!<" + plainText + "]?");
        }

        public static string HashSHA256(this string plainText)
        {
            return Kama.Library.Helper.Security.HashSHA256.Hash(plainText);
        }

        public static string Base64Encrypt(this string plainText)
        {
            return Base64.Encrypt(plainText);
        }

        public static string Base64Decrypt(this string plainText)
        {
            return Base64.Decrypt(plainText);
        }

        public static string RsaEncrypt(this string plainText)
        {
            return Rsa.Encrypt(plainText);
        }

        public static string RsaDecrypt(this string plainText)
        {
            return Rsa.Decrypt(plainText);
        }

        public static string AesEncrypt(this string plainText)
        {
            return Aes.Encrypt(plainText);
        }

        public static string AesDecrypt(this string plainText)
        {
            return Aes.Decrypt(plainText);
        }

        public static string AesEncrypt(this string plainText, string _key, string encryptionSalt)
        {
            return Aes.Encrypt(plainText, _key, encryptionSalt);
        }

        public static string AesDecrypt(this string plainText, string _key)
        {
            return Aes.Decrypt(plainText, _key);
        }

        public static string AesDecrypt(this string ciphertext, string AesEncryptionPassword, string encryptionSalt)
        {
            try
            {
                return Aes.Decrypt(ciphertext.Replace("$$$$$", "/").Replace("^^^^^", "#"), AesEncryptionPassword, encryptionSalt);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
