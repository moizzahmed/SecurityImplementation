using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Utilities.IO.Pem;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityImplementation.Helper
{
    public class clsEncrypt_Decrypt
    {

        public static string Key = "BgSDcMMWW8gKnS55";
        public string HashSalt = "TW5EF1ZoN117DDZ4";



        public static string Encrypt(string PlainText)
        {
            string EncryptedData = Encrypt(PlainText, Key);

            return EncryptedData;
        }


        public static string Decrypt(string EncryptedText)
        {
            string DecryptedData = Decrypt(EncryptedText, Key);

            return DecryptedData;
        }


        private static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            byte[] keyBytes = new byte[16];
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        /// Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <param name="key">Secret key</param>
        /// <returns>Base64 encoded string</returns>
        public static String Encrypt(String plainText, String key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }

        /// <summary>
        /// Decrypts a base64 encoded string using the given key (AES 128bit key and a Chain Block Cipher)
        /// </summary>
        /// <param name="encryptedText">Base64 Encoded String</param>
        /// <param name="key">Secret Key</param>
        /// <returns>Decrypted String</returns>
        public static String Decrypt(String encryptedText, String key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
        }

        //public static string decryptRSA(string cipherText, string pemPath)
        //{
        //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        //    Org.BouncyCastle.OpenSsl.PemReader pr = new Org.BouncyCastle.OpenSsl.PemReader(
        //        (StreamReader)File.OpenText(pemPath)
        //    );
        //    AsymmetricCipherKeyPair keys = (AsymmetricCipherKeyPair)pr.ReadObject();
        //    IAsymmetricBlockCipher cipher = new RsaEngine();
        //    cipher.Init(false, keys.Private);
        //    byte[] deciphered = cipher.ProcessBlock(cipherTextBytes, 0, cipherTextBytes.Length);
        //    return Encoding.UTF8.GetString(deciphered);
        //}
        //public static string decryptRSA_Key(string cipherText, string pemKey)
        //{
        //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        //    TextReader sr = new StringReader(pemKey);
        //    PemReader pr = new PemReader(sr);
        //    AsymmetricCipherKeyPair keys = (AsymmetricCipherKeyPair)pr.ReadObject();
        //    IAsymmetricBlockCipher cipher = new RsaEngine();
        //    cipher.Init(false, keys.Private);
        //    byte[] deciphered = cipher.ProcessBlock(cipherTextBytes, 0, cipherTextBytes.Length);
        //    return Encoding.UTF8.GetString(deciphered);
        //}
    }

}
