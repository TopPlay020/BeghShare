using System.Security.Cryptography;
using System.Text;
namespace BeghShare.Core.Services
{
    //TODO: I need to find somthing more performance !!
    public static class EncryptionService
    {
        private const int KeySizeBytes = 32; // AES-256
        private const int IvSizeBytes = 16;
        private const int Iterations = 1_000;
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("BeghShare-Salt-V1");
        private static readonly byte[] EncryptedHeader = Encoding.UTF8.GetBytes("ENC");
        private static byte[] _cachedKey;
        private static string _cachedPassphrase;
        public static string Passphrase { get; set; } = "BeghShare-Secure-Default-Key";

        public static byte[] Encode(string text, bool encrypt = false)
        {
            if (!encrypt)
                return Encoding.UTF8.GetBytes(text);

            var plainBytes = Encoding.UTF8.GetBytes(text);
            var key = GetKey();
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            var result = new byte[EncryptedHeader.Length + IvSizeBytes + encrypted.Length];
            Buffer.BlockCopy(EncryptedHeader, 0, result, 0, EncryptedHeader.Length);
            Buffer.BlockCopy(aes.IV, 0, result, EncryptedHeader.Length, IvSizeBytes);
            Buffer.BlockCopy(encrypted, 0, result, EncryptedHeader.Length + IvSizeBytes, encrypted.Length);
            return result;
        }

        public static string Decode(byte[] data)
        {
            if (data == null || data.Length < EncryptedHeader.Length)
                return string.Empty;

            bool isEncrypted = true;
            for (int i = 0; i < EncryptedHeader.Length; i++)
            {
                if (data[i] != EncryptedHeader[i])
                {
                    isEncrypted = false;
                    break;
                }
            }

            if (!isEncrypted)
                return Encoding.UTF8.GetString(data);

            if (data.Length < EncryptedHeader.Length + IvSizeBytes + 16)
                return string.Empty;

            var key = GetKey();
            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            var iv = new byte[IvSizeBytes];
            Buffer.BlockCopy(data, EncryptedHeader.Length, iv, 0, IvSizeBytes);
            aes.IV = iv;
            var cipherBytes = new byte[data.Length - EncryptedHeader.Length - IvSizeBytes];
            Buffer.BlockCopy(data, EncryptedHeader.Length + IvSizeBytes, cipherBytes, 0, cipherBytes.Length);
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

        private static byte[] GetKey()
        {
            if (_cachedKey == null || _cachedPassphrase != Passphrase)
            {
                _cachedPassphrase = Passphrase;
                _cachedKey = DeriveKey();
            }
            return _cachedKey;
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