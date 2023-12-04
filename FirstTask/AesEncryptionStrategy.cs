using System.Security.Cryptography;

namespace FirstTask
{
    class AesEncryptionStrategy : IEncryptionStrategy
    {
        public byte[] Encrypt(byte[] data, int length)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                aes.GenerateKey();

                byte[] iv = aes.IV;
                byte[] key = aes.Key;
                byte[] encryptedData;

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, length);
                    cryptoStream.FlushFinalBlock();
                    encryptedData = memoryStream.ToArray();
                }

                OnEncryptionComplete(iv, key);

                return encryptedData;
            }
        }

        private void OnEncryptionComplete(byte[] iv, byte[] key)
        {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(byte[] data, int length)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.IV.Length];
                byte[] key = new byte[aes.Key.Length];
                byte[] decryptedData;

                Buffer.BlockCopy(data, 0, iv, 0, aes.IV.Length);
                Buffer.BlockCopy(data, aes.IV.Length, key, 0, aes.Key.Length);

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, aes.IV.Length + aes.Key.Length, length);
                    cryptoStream.FlushFinalBlock();
                    decryptedData = memoryStream.ToArray();
                }

                OnDecryptionComplete(iv, key);

                return decryptedData;
            }
        }

        private void OnDecryptionComplete(byte[] iv, byte[] key)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IEncryptionStrategy
    {
    }
}
