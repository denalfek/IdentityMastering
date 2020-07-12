using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IdentityMastering
{
    public static class Protector
    {
        private const string _rawSalt = "Let's check this practice";
        private const int _iterations = 2000;
        private const int _256BitKey = 32;
        private const int _128BitKey = 16;

        private static readonly Aes _aes = Aes.Create();
        private static readonly byte[] _salt =
            Encoding.Unicode.GetBytes(_rawSalt);

        public static string Encrypt(string plainText, string password)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.Unicode
                .GetBytes(plainText);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, _salt, _iterations);
            _aes.Key = pbkdf2.GetBytes(_256BitKey);
            _aes.IV = pbkdf2.GetBytes(_128BitKey);

            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, _aes.CreateEncryptor(), CryptoStreamMode.Write);
            const int offset = 0;
            cryptoStream.Write(plainBytes, offset, plainBytes.Length);
            encryptedBytes = memoryStream.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cryptoText, string password)
        {
            byte[] plainBytes;
            byte[] cryptoBytes =
                Convert.FromBase64String(cryptoText);

            var pbkdf2 = new Rfc2898DeriveBytes(password, _salt, _iterations);
            _aes.Key = pbkdf2.GetBytes(_256BitKey);
            _aes.IV = pbkdf2.GetBytes(_128BitKey);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, _aes.CreateDecryptor(), CryptoStreamMode.Write);
            const int offset = 0;
            cryptoStream.Write(cryptoBytes, offset, cryptoBytes.Length);
            plainBytes = memoryStream.ToArray();
            return Encoding.Unicode.GetString(plainBytes);
        }

        // private static string Pocess
    }
}
