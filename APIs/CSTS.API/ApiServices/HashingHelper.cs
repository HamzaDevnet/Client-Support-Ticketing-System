using System.Security.Cryptography;
using System.Text;

namespace CSTS.API.ApiServices
{
    public static class HashingHelper
    {
        private static string salt = "Improve-security-add-salt";

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(salt, inputString)));
        }


        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(string.Concat(salt, inputString)))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


        public static bool CompareHash(string attemptedPassword, string hashed)
        {
            string base64Hash = Convert.ToBase64String(hashed);
            string base64AttemptedHash = Convert.ToBase64String(GetHashString(string.Concat(salt, attemptedPassword)));

            return base64Hash == base64AttemptedHash;
        }

    }
}
