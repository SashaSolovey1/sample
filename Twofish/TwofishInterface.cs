using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

namespace Twofish
{
    public static class TwofishInterface
    {
        [ThreadStatic] private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();
        private static readonly byte[] iv = { 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 2 };
        public static string GenerateRandomKey()
        {
            byte[] bKey = new byte[16];
            Rng.GetBytes(bKey);
            return Convert.ToBase64String(bKey);
        }
        public static string Crypt(string data, string skey)
        {
            var bIn = Encoding.UTF8.GetBytes(data);
            var key = Convert.FromBase64String(skey);

            using (var algorithm = new TwofishManaged { KeySize = key.Length * 8, Mode = CipherMode.CBC })
            {
                using (var ms = new MemoryStream())
                {
                    using (var transform = algorithm.CreateEncryptor(key, iv))
                    {
                        using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(bIn, 0, bIn.Length);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string Decrypt(string data, string skey)
        {
            var encrypted = Convert.FromBase64String(data);
            var key = Convert.FromBase64String(skey);

            using (var algorithm = new TwofishManaged { KeySize = key.Length * 8, Mode = CipherMode.CBC })
            {
                using (var ms = new MemoryStream())
                {
                    using (var transform = algorithm.CreateDecryptor(key, iv))
                    {
                        using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(encrypted, 0, encrypted.Length);
                        }
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
