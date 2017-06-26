using System;
using System.Security.Cryptography;
using System.Text;

namespace LightningTalk.Utils
{
    internal static class Gravatar
    {
        internal static string GetImageUrl(string email)
        {
            return "https://www.gravatar.com/avatar/" + GetConsistentHash(email);
        }

        private static string GetConsistentHash(string email)
        {
            return email.Trim().ToLowerInvariant().GetMD5().ToLowerInvariant();
        }

        private static string GetMD5(this string email)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
