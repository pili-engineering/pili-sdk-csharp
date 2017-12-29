using System.Security.Cryptography;
using System.Text;

namespace Qiniu.Pili.Internal
{
    internal static class HMACSHA1Extensions
    {
        public static byte[] ComputeHash(this HMACSHA1 hmacsha1, string input)
        {
            return hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}
