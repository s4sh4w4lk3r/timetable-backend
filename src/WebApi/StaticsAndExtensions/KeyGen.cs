using System.Security.Cryptography;
using System.Text;

namespace WebApi.StaticsAndExtensions
{
    public static class KeyGen
    {
        private const string _prefix = "TT-";
        private const int _numberOfSecureBytesToGenerate = 32;

        public static string Generate(int lengthOfKey = 64)
        {
            var bytes = RandomNumberGenerator.GetBytes(_numberOfSecureBytesToGenerate);

            string base64String = Convert.ToBase64String(bytes);

            base64String = new StringBuilder(base64String)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "%")
                .Replace("&", "$")
                .Replace("?", "*")
                .ToString();

            var keyLength = lengthOfKey - _prefix.Length;

            return _prefix + base64String[..keyLength];
        }
    }
}
