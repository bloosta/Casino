using System.Security.Cryptography;
using System.Text;

namespace CasinoConsoleApp.Core.Security
{
    public class PasswordHasher
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool Verify(string password, string hash)
        {
            var computedHash = Hash(password);
            return computedHash == hash;
        }
    }
}
