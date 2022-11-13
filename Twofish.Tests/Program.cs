using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Twofish.Tests
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.Title = "Twofish.Tests";

            var generator = RandomNumberGenerator.Create();

            byte[] bKey = new byte[16];
            generator.GetBytes(bKey);

            var strKey = Convert.ToBase64String(bKey);
            var str = "It works!";
            var encrypted = TwofishInterface.Crypt(str, strKey);
            var decrypted = TwofishInterface.Decrypt(encrypted, strKey);
            Console.WriteLine($"Key: {strKey}");
            Console.WriteLine($"Input data: {str}");
            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");

            Console.Read();
        }
    }
}