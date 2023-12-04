using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static Stream fileStream;

    public static ICryptoTransform decryptor { get; private set; }

    static void Main(string[] args)
    {
        string inputFilePath = "input.txt";
        string outputFilePath = "output.txt";

        Console.WriteLine("1. Encrypt");
        Console.WriteLine("2. Decrypt");
        Console.Write("Choose an option: ");
        int option = Convert.ToInt32(Console.ReadLine());

        switch (option)
        {
            case 1:
                EncryptFile(inputFilePath, outputFilePath);
                break;
            case 2:
                DecryptFile(inputFilePath, outputFilePath);
                break;
            default:
                Console.WriteLine("Invalid option");
                break;
        }
    }

    static void EncryptFile(string inputFilePath, string outputFilePath)
    {
        Console.WriteLine("Encrypting file...");

        using (FileStream inputStream = File.OpenRead(inputFilePath))
        using (FileStream outputStream = File.OpenWrite(outputFilePath))
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                byte[] encryptedData = Encrypt(buffer, bytesRead);
                outputStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        Console.WriteLine("Encryption complete");
    }

    static void DecryptFile(string inputFilePath, string outputFilePath)
    {
        Console.WriteLine("Decrypting file...");

        using (FileStream fileStream = File.OpenRead(inputFilePath))
        using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create))
        {
            byte[] iv = new byte[16];
            byte[] key = new byte[32];

            if (fileStream.Read(iv, 0, iv.Length) != iv.Length)
            {
                Console.WriteLine("Invalid IV");
                return;
            }

            if (fileStream.Read(key, 0, key.Length) != key.Length)
            {
                Console.WriteLine("Invalid Key");
                return;
            }

            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;

                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[16];
                    int bytesRead;
                    while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        Console.WriteLine("Decryption complete");
    }

    static byte[] Encrypt(byte[] data, int length)
    {
        IEncryptionStrategy encryptionStrategy = new AesEncryptionStrategy();

        return encryptionStrategy.Encrypt(data, length);
    }

    static byte[] Decrypt(byte[] data, int length)
    {
        IEncryptionStrategy encryptionStrategy = new AesEncryptionStrategy();

        return encryptionStrategy.Decrypt(data, length);
    }

    interface IEncryptionStrategy
    {
        byte[] Encrypt(byte[] data, int length);
        byte[] Decrypt(byte[] data, int length);
    }

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
    }

    static void OnEncryptionComplete(byte[] iv, byte[] key)
    {
        Console.WriteLine("Encryption completed.");
        Console.WriteLine("IV: " + Convert.ToBase64String(iv));
        Console.WriteLine("Key: " + Convert.ToBase64String(key));
    }

    static void OnDecryptionComplete(byte[] iv, byte[] key)
    {
        Console.WriteLine("Decryption completed.");
        Console.WriteLine("IV: " + Convert.ToBase64String(iv));
        Console.WriteLine("Key: " + Convert.ToBase64String(key));
    }
}