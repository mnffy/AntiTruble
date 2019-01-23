using AntiTruble.ClassLibrary;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AntiTruble.Person.Extentions
{
    public class SecurePasswordHasher
    {
        public static string Encrypt(string password)
        {
            var clearBytes = Encoding.Unicode.GetBytes(password);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(Scope.EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        public static string Decrypt(string password)
        {
            var cipherBytes = Convert.FromBase64String(password);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(Scope.EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return password;
        }
    }
}
