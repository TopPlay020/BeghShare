using System.Security.Cryptography;
using System.Text;

namespace BeghShare.Core.Services
{
    public static class EncryptionService
    {
        private const int KeySizeBytes = 32; // AES-256
        private const int IvSizeBytes = 16;
        private const int Iterations = 100_000;
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("BeghShare-Salt-V1");

        /// <summary>Pre-shared passphrase for AES. Set before first use to customize the key.</summary>
        public static string Passphrase { get; set; } = "BeghShare-Secure-Default-Key";

        public static byte[] Encode(string text)
        {
            var plainBytes = Encoding.UTF8.GetBytes(text);
            var key = DeriveKey();

            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[IvSizeBytes + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, IvSizeBytes);
            Buffer.BlockCopy(encrypted, 0, result, IvSizeBytes, encrypted.Length);
            return result;
        }

        public static string Decode(byte[] data)
        {
            if (data == null || data.Length < IvSizeBytes + 16)
                return string.Empty;

            var key = DeriveKey();

            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var iv = new byte[IvSizeBytes];
            Buffer.BlockCopy(data, 0, iv, 0, IvSizeBytes);
            aes.IV = iv;

            var cipherBytes = new byte[data.Length - IvSizeBytes];
            Buffer.BlockCopy(data, IvSizeBytes, cipherBytes, 0, cipherBytes.Length);

            try
            {
                using var decryptor = aes.CreateDecryptor();
                var decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
        }

        private static byte[] DeriveKey()
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                Passphrase,
                Salt,
                Iterations);

            return pbkdf2.GetBytes(KeySizeBytes);
        }

    }
}
