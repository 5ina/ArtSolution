using Abp.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ArtSolution.Security
{
    public class EncryptionService : ArtSolutionAppServiceBase, IEncryptionService
    {
        #region method

        public virtual string CreateSaltKey(int size)
        {
            //生成加密随机数
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            //返回一个随机数的Base64字符串表示形式
            return Convert.ToBase64String(buff);
        }

        public string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SettingManager.GetSettingValue("EncryptionKey");

            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SettingManager.GetSettingValue("EncryptionKey");

            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] buffer = Convert.FromBase64String(cipherText);
            return DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
        }
        #endregion

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = new UnicodeEncoding().GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    var sr = new StreamReader(cs, new UnicodeEncoding());
                    return sr.ReadLine();
                }
            }
        }

        #endregion
    }
}
